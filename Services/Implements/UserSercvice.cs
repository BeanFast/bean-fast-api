using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Account.Request;
using DataTransferObjects.Account.Response;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Util;
using Utilities.Utils;

namespace Services.Implements
{
    public class UserSercvice : BaseService<User>, IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        public UserSercvice(IUnitOfWork<BeanFastContext>? unitOfWork) : base(unitOfWork)
        {
            _userRepository = unitOfWork.GetRepository<User>();
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            Expression<Func<User, bool>> whereFilter = (user) =>
                user.Phone == loginRequest.Phone &&
                PasswordUtil.VerifyPassword(user.Password, loginRequest.Password!);
            User user = await _userRepository.FirstOrDefaultAsync(predicate: whereFilter) ?? throw new Exception();

            return new LoginResponse
            {
                AccessToken = JwtUtil.GenerateToken(user)
            };
        }
    }
}
