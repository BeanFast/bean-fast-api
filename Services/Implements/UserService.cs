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
using DataTransferObjects.Models.User.Request;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using DataTransferObjects.Models.Notification.Request;

namespace Services.Implements
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IRoleService _roleService;
        private readonly ISmsOtpService _smsOtpService;
        private readonly IWalletService _walletService;
        //private readonly INotificationService _notificationService;
        public UserService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings,
            ICloudStorageService cloudStorageService, IRoleService roleService, ISmsOtpService smsOtpService, IWalletService walletService
            //, INotificationService notificationService
            ) : base(
            unitOfWork, mapper, appSettings)
        {
            _cloudStorageService = cloudStorageService;
            _roleService = roleService;
            _smsOtpService = smsOtpService;
            _walletService = walletService;
            //_notificationService = notificationService;
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {
            List<Expression<Func<User, bool>>> filters = new()
            {
                (user) => user.Id == userId,
                //(user) => user.Status == BaseEntityStatus.Active
            };

            var user = await _repository.FirstOrDefaultAsync(filters: filters,
                include: queryable => queryable.Include(u => u.Role!).Include(u => u.Wallets!))
                ?? throw new EntityNotFoundException(MessageConstants.UserMessageConstrant.UserNotFound(userId));
            if(UserStatus.NotVerified == user.Status) 
            {
                throw new NotVerifiedAccountException();    
            }
            if (UserStatus.Deleted == user.Status)
            {
                throw new BannedAccountException();
            }
            return user;
        }
        public async Task<User> GetCustomerByQrCodeAsync(string qrCode)
        {
            List<Expression<Func<User, bool>>> filters = new()
            {
                (user) => user.QRCode == qrCode && user.Status == BaseEntityStatus.Active,
                //(user) => user.QrCodeExpiry > TimeUtil.GetCurrentVietNamTime()
            };
            var user = await _repository.FirstOrDefaultAsync(
                filters: filters,
                include: queryable => queryable.Include(u => u.Role!).Include(u => u.Wallets!))
                ?? throw new EntityNotFoundException(MessageConstants.UserMessageConstrant.UserNotFoundByQrCode);
            if (user.QrCodeExpiry < TimeUtil.GetCurrentVietNamTime())
            {
                throw new InvalidRequestException(MessageConstants.UserMessageConstrant.QrCodeExpired);
            }
            return user;
        }
        public async Task<ICollection<GetDelivererResponse>> GetDeliverersExcludeAsync(List<Guid> excludeDelivererIds)
        {
            List<Expression<Func<User, bool>>> filters = new()
            {
                (user) => RoleName.DELIVERER.ToString().Equals(user.Role!.EnglishName),
                (user) => !excludeDelivererIds.Contains(user.Id),
                //(user) => user.SessionDetails != null && user.SessionDetails.Count == 0,

            };
            var users = await _repository.GetListAsync<GetDelivererResponse>(filters: filters);
            return users;
        }
        public async Task<ICollection<GetDelivererResponse>> GetDeliverersAsync()
        {
            List<Expression<Func<User, bool>>> filters = new()
            {
                (user) => RoleName.DELIVERER.ToString().Equals(user.Role!.EnglishName),

            };
            var users = await _repository.GetListAsync<GetDelivererResponse>(filters: filters);
            return users;
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
                        throw new InvalidRequestException(MessageConstants.LoginMessageConstrant.InvalidCredentials);
            if (!PasswordUtil.VerifyPassword(loginRequest.Password, user.Password))
            {
                throw new InvalidRequestException(MessageConstants.LoginMessageConstrant.InvalidCredentials);
            }

            if (user.Status == BaseEntityStatus.Deleted)
            {
                throw new BannedAccountException();
            }else if (user.Status == UserStatus.NotVerified)
            {
                throw new NotVerifiedAccountException();
            }
            if (!loginRequest.DeviceToken.IsNullOrEmpty())
            {
                user.DeviceToken = loginRequest.DeviceToken;
                await _repository.UpdateAsync(user);
                await _unitOfWork.CommitAsync();
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
                throw new InvalidRequestException(MessageConstants.AuthorizationMessageConstrant.DupplicatedPhone);
            var customerRole = await _roleService.GetRoleByRoleNameAsync(RoleName.CUSTOMER);
            customer.Password = PasswordUtil.HashPassword(registerRequest.Password);
            customer.Id = Guid.NewGuid();
            customer.Status = BaseEntityStatus.Active;
            customer.AvatarPath = UserConstrants.DefaultAvatar;
            customer.RoleId = customerRole.Id;
            customer.Status = UserStatus.NotVerified;

            var customerNumber = await _repository.CountAsync() + 1;
            customer.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.UserCodeConstrant.CustomerPrefix, customerNumber);
            var moneyWallet = new Wallet
            {
                Id = Guid.NewGuid(),
                Name = customer.FullName!,
                UserId = customer.Id,
            };
            await _repository.InsertAsync(customer);
            await _unitOfWork.CommitAsync();
            await _walletService.CreateWalletAsync(WalletType.Money, moneyWallet);
            return new RegisterResponse();
        }

        public async Task SendOtpAsync(string phone)
        {
            phone = phone.Replace("+84", "0");
            var user = await findNotVerifiedUserByPhone(phone);
            await _smsOtpService.SendOtpAsync(user);
        }

        public async Task UpdateCustomerAsync(UpdateCustomerRequest request, User user)
        {
            user.FullName = request.FullName;
            if (request.Image != null)
            {
                user.AvatarPath = await _cloudStorageService.UploadFileAsync(user.Id, _appSettings.Firebase.FolderNames.User, request.Image);
            }
            await _repository.UpdateAsync(user);
            await _unitOfWork.CommitAsync();
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
                throw new InvalidRequestException(MessageConstants.AuthorizationMessageConstrant.InvalidSmsOtp);
            }
            return verifyResult;
        }

        private async Task<User> findNotVerifiedUserByPhone(string phone)
        {
            return await _repository.FirstOrDefaultAsync(
                filters: new List<Expression<Func<User, bool>>>
                {
                    (user) => user.Phone == phone,
                    (user) => user.Status == UserStatus.NotVerified,
                }
            ) ?? throw new EntityNotFoundException(MessageConstants.AuthorizationMessageConstrant.PhoneNotFound);
        }

        public async Task CreateUserAsync(CreateUserRequest request)
        {
            var user = _mapper.Map<User>(request);
            var role = await _roleService.GetRoleByIdAsync(request.RoleId);
            if (RoleName.CUSTOMER.ToString().Equals(role.EnglishName)) throw new InvalidRequestException("");
            var userId = Guid.NewGuid();
            var checkDupplicatedUserData = await _repository.FirstOrDefaultAsync(filters: new List<Expression<Func<User, bool>>>()
            {
                u => u.Email == request.Email ,
            });
            if (checkDupplicatedUserData is not null) throw new InvalidRequestException(MessageConstants.AuthorizationMessageConstrant.DupplicatedEmail);
            user.AvatarPath = await _cloudStorageService.UploadFileAsync(userId, _appSettings.Firebase.FolderNames.User, request.Image);
            user.Status = UserStatus.Active;
            var userNumber = await _repository.CountAsync() + 1;
            user.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.UserCodeConstrant.UserPrefix, userNumber);
            user.Password = PasswordUtil.HashPassword(request.Password);
            await _repository.UpdateAsync(user);
            await _unitOfWork.CommitAsync();
        }
        public async Task<User> GetUserByIdIncludeWallet(Guid userId)
        {
            List<Expression<Func<User, bool>>> filters = new()
            {
                (user) => user.Id == userId
            };

            var user = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active,
                filters: filters,
                include: queryable => queryable.Include(u => u.Role!).Include(u => u.Wallets!))
                ?? throw new EntityNotFoundException(MessageConstants.UserMessageConstrant.UserNotFound(userId));
            return user;
        }
        public async Task<GetCurrentUserResponse> GetCurrentUserAsync(Guid userId)
        {
            var user = await GetUserByIdIncludeWallet(userId);
            var mappedUser = _mapper.Map<GetCurrentUserResponse>(user);
            //mappedUser.Balance = user.Wallets?.FirstOrDefault(w => WalletType.Money.ToString().Equals(w.Type))?.Balance;
            if (user.Wallets != null && user.Wallets.Any())
            {
                mappedUser.Balance = user.Wallets.FirstOrDefault(w => WalletType.Money.ToString().Equals(w.Type))!.Balance;
            }
            return mappedUser;

        }

        public async Task<GenerateQrCodeResponse> GenerateQrCodeAsync(User user)
        {
            var qrCodeString = "";
            DateTime currentVietnamTime;
            do
            {
                currentVietnamTime = TimeUtil.GetCurrentVietNamTime();
                qrCodeString = QrCodeUtil.GenerateQRCodeString(user.Id.ToString() + user.Code + currentVietnamTime, _appSettings.QrCode.QrCodeSecretKey);
            } while (await _repository.FirstOrDefaultAsync(filters: new List<Expression<Func<User, bool>>>()
            {
                    u => u.QRCode == qrCodeString
            }) != null);
            user.QRCode = qrCodeString;
            user.QrCodeExpiry = currentVietnamTime.AddSeconds(_appSettings.QrCode.QrCodeExpiryInSeconds);
            await _repository.UpdateAsync(user);
            await _unitOfWork.CommitAsync();
            return new GenerateQrCodeResponse
            {
                QrCode = qrCodeString,
                QrCodeExpiry = user.QrCodeExpiry.Value
            };
        }

        public Task<GetUserResponse> GetUserResponseByIdAsync(Guid userId)
        {
            var result = _repository.FirstOrDefaultAsync<GetUserResponse>(filters: new ()
            {
                u => u.Id == userId
            });
            if (result == null) throw new EntityNotFoundException(MessageConstants.UserMessageConstrant.UserNotFound(userId));
            return result!;
        }

        public Task<ICollection<GetUserResponse>> GetAllAsync(UserFilterRequest request)
        {
            var filters = new List<Expression<Func<User, bool>>>();
            if(request.RoleId != null && request.RoleId != Guid.Empty)
            {
                filters.Add(u => u.RoleId == request.RoleId);
            }
            var result = _repository.GetListAsync<GetUserResponse>(filters: filters);
            return result;
        }

        public async Task UpdateUserStatusAsync(Guid id, UpdateUserStatusRequest request)
        {
            var user = await _repository.FirstOrDefaultAsync(filters: new List<Expression<Func<User, bool>>>()
            {
                u => u.Id == id
            }) ?? throw new EntityNotFoundException(MessageConstants.UserMessageConstrant.UserNotFound(id));
            if(request.IsActive)
            {
                user.Status = UserStatus.Active;
            }
            else if(!request.IsActive)
            {
                user.Status = UserStatus.Deleted;
            }
            await _repository.UpdateAsync(user);
            await _unitOfWork.CommitAsync();
        }
    }
}