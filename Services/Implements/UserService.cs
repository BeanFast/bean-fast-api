using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Account.Request;
using DataTransferObjects.Account.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Linq.Expressions;
using Utilities.Utils;

namespace Services.Implements
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IGenericRepository<User> _userRepository;

        public UserService(IUnitOfWork<BeanFastContext> unitOfWork) : base(unitOfWork)
        {
            _userRepository = unitOfWork.GetRepository<User>();
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            Expression<Func<User, bool>> whereFilter = (user) =>
                user.Phone == loginRequest.Phone;
            Func<IQueryable<User>, IIncludableQueryable<User, object>>  include = (user) => user.Include(u => u.Role!);
            User user = await _userRepository.FirstOrDefaultAsync(predicate: whereFilter, include: include) ?? throw new Exception();
            if (!PasswordUtil.VerifyPassword(loginRequest.Password!, user.Password))
            {
                throw new Exception();
            }

            return new LoginResponse
            {
                AccessToken = JwtUtil.GenerateToken(user)
            };
        }
    }
}