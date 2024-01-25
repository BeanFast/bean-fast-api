using DataTransferObjects.Account.Request;
using DataTransferObjects.Account.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
    }
}
