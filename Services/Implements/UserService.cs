using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Auth.Request;
using DataTransferObjects.Models.Auth.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Linq.Expressions;
using AutoMapper;
using Utilities.Utils;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Constants;
using Microsoft.Extensions.Options;
using Utilities.Settings;
using Utilities.Statuses;
using DataTransferObjects.Models.User.Response;
using DataTransferObjects.Models.SmsOtp;

namespace Services.Implements
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly ICloudStorageService _cloudStorageService;

        private readonly IRoleService _roleService;
        private readonly ISmsOtpService _smsOtpService;

        public UserService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings,
            ICloudStorageService cloudStorageService, IRoleService roleService, ISmsOtpService smsOtpService) : base(
            unitOfWork, mapper, appSettings)
        {
            _cloudStorageService = cloudStorageService;
            _roleService = roleService;
            _smsOtpService = smsOtpService;
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {
            List<Expression<Func<User, bool>>> filters = new()
            {
                (user) => user.Id == userId
            };

            var user = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active,
                filters: filters,
                include: queryable => queryable.Include(u => u.Role!))
                ?? throw new EntityNotFoundException(MessageConstants.UserMessageConstrant.UserNotFound(userId));
            return user;
        }

        public async Task<GetDelivererResponse> GetDelivererResponseById(Guid id)
        {
            return _mapper.Map<GetDelivererResponse>(await GetByIdAsync(id));
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            List<Expression<Func<User, bool>>>? whereFilters = null;
            if (loginRequest.Email is not null)
            {
                whereFilters = new()
                {
                    user => user.Email == loginRequest.Email && user.Role!.EnglishName != RoleName.CUSTOMER.ToString()
                };
            }
            else if (loginRequest.Phone is not null)
            {
                whereFilters = new()
                {
                    (user) => user.Phone == loginRequest.Phone && user.Role!.EnglishName == RoleName.CUSTOMER.ToString()
                };
            }

            Func<IQueryable<User>, IIncludableQueryable<User, object>> include = (user) => user.Include(u => u.Role!);
            User user = await _repository.FirstOrDefaultAsync(filters: whereFilters, include: include) ??
                        throw new InvalidCredentialsException();
            if (!PasswordUtil.VerifyPassword(loginRequest.Password, user.Password))
            {
                throw new InvalidCredentialsException();
            }

            if (user.Status == BaseEntityStatus.Deleted)
            {
                throw new BannedAccountException();
            }

            return new LoginResponse
            {
                AccessToken = JwtUtil.GenerateToken(user)
            };
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            var customer = _mapper.Map<User>(registerRequest);
            List<Expression<Func<User, bool>>> filters = new()
            {
                (user) => user.Phone == registerRequest.Phone
            };
            var existedData = await _repository.FirstOrDefaultAsync(
                status: BaseEntityStatus.Active,
                filters: filters
            );
            if (existedData is not null)
                throw new DataExistedException(MessageConstants.AuthorizationMessageConstrant.DupplicatedPhone);
            customer.Password = PasswordUtil.HashPassword(registerRequest.Password);
            customer.Id = Guid.NewGuid();
            customer.Status = BaseEntityStatus.Active;
            var avatarPath = await _cloudStorageService.UploadFileAsync(customer.Id,
                _appSettings.Firebase.FolderNames.User, registerRequest.Image);
            customer.AvatarPath = avatarPath;
            customer.Role = await _roleService.GetRoleByRoleNameAsync(RoleName.CUSTOMER);
            customer.Status = UserStatus.NotVerified;
            var customerNumber = await _repository.CountAsync() + 1;
            customer.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.UserCodeConstrant.CustomerPrefix, customerNumber);

            await _repository.InsertAsync(customer);
            await _unitOfWork.CommitAsync();
            return new RegisterResponse();
        }

        public async Task SendOtpAsync(string phone)
        {
            phone = phone.Replace("+84", "0");
            var user = await findNotVerifiedUserByPhone(phone);
            await _smsOtpService.SendOtpAsync(user);
        }

        public async Task<bool> VerifyOtpAsync(SmsOtpVerificationRequest request)
        {
            var user = await findNotVerifiedUserByPhone(request.Phone);
            var verifyResult = await _smsOtpService.VerifyOtpAsync(request, user);
            if (verifyResult)
            {
                user.Status = UserStatus.Active;
                await _repository.UpdateAsync(user);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new InvalidOtpException();
            }
            return verifyResult;
        }

        private async Task<User> findNotVerifiedUserByPhone(string phone)
        {
            return await _repository.FirstOrDefaultAsync(
                status: UserStatus.NotVerified,
                filters: new List<Expression<Func<User, bool>>>
                {
                    (user) => user.Phone == phone,
                    (user) => user.Status == UserStatus.NotVerified,
                }
            ) ?? throw new EntityNotFoundException(MessageConstants.AuthorizationMessageConstrant.PhoneNotFound);
        }


    }
}