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

namespace Services.Implements
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IGenericRepository<User> _userRepository;

        public UserService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _userRepository = unitOfWork.GetRepository<User>();
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            Expression<Func<User, bool>> whereFilter;
            if (loginRequest.Role == RoleName.ADMIN || loginRequest.Role == RoleName.MANAGER)
            {
                if (loginRequest.Email == null) throw new InvalidCredentialsException(MessageContants.Login.EmailRequired);
                whereFilter = (user) => user.Email == loginRequest.Email;
            }
            else if (loginRequest.Role == RoleName.CUSTOMER)
            {
                if (loginRequest.Phone == null) throw new InvalidCredentialsException(MessageContants.Login.EmailRequired);
                whereFilter = (user) => user.Phone == loginRequest.Phone;
            }
            else
            {
                throw new ArgumentException("Invalid user role");
            }
            Func<IQueryable<User>, IIncludableQueryable<User, object>> include = (user) => user.Include(u => u.Role!);
            User user = await _userRepository.FirstOrDefaultAsync(predicate: whereFilter, include: include) ?? throw new InvalidCredentialsException();
            if (!PasswordUtil.VerifyPassword(loginRequest.Password!, user.Password))
            {
                throw new InvalidCredentialsException();
            }

            return new LoginResponse
            {
                AccessToken = JwtUtil.GenerateToken(user)
            };
        }

        public Task<RegisterResponse> Register(RegisterRequest registerRequest)
        {
            return null;
        }
    }
}