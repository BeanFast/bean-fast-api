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
using System.IdentityModel.Tokens.Jwt;
using Twilio.Jwt.AccessToken;

namespace Services.Implements
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IRoleService _roleService;
        private readonly ISmsOtpService _smsOtpService;
        private readonly IWalletService _walletService;
        private readonly IUserRepository _repository;
        //private readonly INotificationService _notificationService;
        public UserService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings,
            ICloudStorageService cloudStorageService, IRoleService roleService, ISmsOtpService smsOtpService, IWalletService walletService
, IUserRepository repository
            //, INotificationService notificationService
            ) : base(
            unitOfWork, mapper, appSettings)
        {
            _cloudStorageService = cloudStorageService;
            _roleService = roleService;
            _smsOtpService = smsOtpService;
            _walletService = walletService;
            _repository = repository;
            //_notificationService = notificationService;
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {
            return await _repository.GetByIdAsync(userId);
        }
        public async Task<User> GetManagerByIdAsync(Guid managerId)
        {
            return await _repository.GetManagerByIdAsync(managerId);
        }
        public async Task<User> GetCustomerByQrCodeAsync(string qrCode)
        {
            var user = await _repository.GetCustomerByQrCodeAsync(qrCode);
            if (user == null) throw new EntityNotFoundException(MessageConstants.UserMessageConstrant.UserNotFoundByQrCode);
            if (user.QrCodeExpiry < TimeUtil.GetCurrentVietNamTime())
            {
                throw new InvalidRequestException(MessageConstants.UserMessageConstrant.QrCodeExpired);
            }
            return user;
        }
        public async Task<ICollection<GetDelivererResponse>> GetDeliverersExcludeAsync(List<Guid> excludeDelivererIds)
        {
            return await _repository.GetDeliverersExcludeAsync(excludeDelivererIds);
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
            var user = await _repository.LoginAsync(loginRequest);
            var refreshToken = JwtUtil.GenerateRefreshToken(user);
            user.RefreshToken = refreshToken;
            await _repository.UpdateAsync(user);
            await _unitOfWork.CommitAsync();
            return new LoginResponse
            {
                AccessToken = JwtUtil.GenerateToken(user),
                RefreshToken = refreshToken
            };
        }

        public async Task<LoginResponse> RefreshTokenAsync(string refreshToken, User user)
        {
            if (user.RefreshToken != refreshToken)
            {
                throw new InvalidRequestException(MessageConstants.AuthorizationMessageConstrant.InvalidRefreshToken);
            }
            var tokenValidated = JwtUtil.ValidateRefreshToken(refreshToken);
            if (!tokenValidated)
            {
                throw new InvalidRequestException(MessageConstants.AuthorizationMessageConstrant.InvalidRefreshToken);
            }
            var newRefreshToken = JwtUtil.GenerateRefreshToken(user);
            user.RefreshToken = newRefreshToken;
            await _repository.UpdateAsync(user);
            await _unitOfWork.CommitAsync();
            return new LoginResponse
            {
                AccessToken = JwtUtil.GenerateToken(user),
                RefreshToken = refreshToken
            };
        }
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            var customer = _mapper.Map<User>(registerRequest);

            var existedData = await _repository.FindUserByPhone(registerRequest.Phone);
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
                Name = "Ví tiền của: #" + customer.Id,
                UserId = customer.Id,
            };
            var pointsWallet = new Wallet
            {
                Id = Guid.NewGuid(),
                Name = "Ví điểm của: #" + customer.Id,
                UserId = customer.Id,
            };
            await _repository.InsertAsync(customer);
            await _unitOfWork.CommitAsync();
            await _walletService.CreateWalletAsync(WalletType.Money, moneyWallet);
            await _walletService.CreateWalletAsync(WalletType.Points, pointsWallet);
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
            return await _repository.FindNotVerifiedUserByPhone(phone);
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
            return await _repository.GetUserByIdIncludeWallet(userId);
        }
        public async Task<GetCurrentUserResponse> GetCurrentUserAsync(Guid userId)
        {
            var user = await GetUserByIdIncludeWallet(userId);
            var mappedUser = _mapper.Map<GetCurrentUserResponse>(user);
            //mappedUser.Balance = user.Wallets?.FirstOrDefault(w => WalletType.Money.ToString().Equals(w.Type))?.Balance;
            if (user.Wallets != null && user.Wallets.Any())
            {
                mappedUser.Balance = user.Wallets.FirstOrDefault(w => WalletType.Money.ToString().Equals(w.Type))!.Balance;
                mappedUser.Points = user.Wallets.FirstOrDefault(w => WalletType.Points.ToString().Equals(w.Type))!.Balance;
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
            } while (
                await _repository.GetCustomerByQrCodeAsync(qrCodeString) != null
            );
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

        public async Task<GetUserResponse> GetUserResponseByIdAsync(Guid userId)
        {
            return await _repository.GetUserResponseByIdAsync(userId);
        }

        public async Task<ICollection<GetUserResponse>> GetAllAsync(UserFilterRequest request)
        {
            return await _repository.GetAllAsync(request);
        }

        public async Task UpdateUserStatusAsync(Guid id, UpdateUserStatusRequest request)
        {
            var user = await _repository.FirstOrDefaultAsync(filters: new List<Expression<Func<User, bool>>>()
            {
                u => u.Id == id
            }) ?? throw new EntityNotFoundException(MessageConstants.UserMessageConstrant.UserNotFound(id));
            if (request.IsActive)
            {
                user.Status = UserStatus.Active;
            }
            else if (!request.IsActive)
            {
                user.Status = UserStatus.Deleted;
            }
            await _repository.UpdateAsync(user);
            await _unitOfWork.CommitAsync();
        }
    }
}