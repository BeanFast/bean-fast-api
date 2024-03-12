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

namespace Services.Implements
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IGenericRepository<User> _userRepository;

        public UserService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
            _userRepository = unitOfWork.GetRepository<User>();
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            List<Expression<Func<User, bool>>> whereFilters = null;
            if (loginRequest.Email is not null)
            {
                whereFilters = new()
                    { user => user.Email == loginRequest.Email && user.Role!.Name != RoleName.CUSTOMER.ToString() };
            }
            else if (loginRequest.Phone is not null)
            {
                whereFilters = new()
                {
                    (user) => user.Phone == loginRequest.Phone //&& user.Role!.Name == RoleName.CUSTOMER.ToString()
                };
            }

            Func<IQueryable<User>, IIncludableQueryable<User, object>> include = (user) => user.Include(u => u.Role!);
            User user = await _userRepository.FirstOrDefaultAsync(filters: whereFilters, include: include) ??
                        throw new InvalidCredentialsException();
            if (!PasswordUtil.VerifyPassword(loginRequest.Password, user.Password))
            {
                throw new InvalidCredentialsException();
            }

            return new LoginResponse
            {
                AccessToken = JwtUtil.GenerateToken(user)
            };
        }

        public Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            return null;
        }
    }
}