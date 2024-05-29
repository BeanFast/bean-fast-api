using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Auth.Response;
using DataTransferObjects.Models.User.Response;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Linq.Expressions;
using Utilities.Constants;
using Utilities.Exceptions;
using Utilities.Statuses;
using Utilities.Utils;
using Microsoft.IdentityModel.Tokens;
using Utilities.Enums;
using DataTransferObjects.Models.User.Request;
using DataTransferObjects.Models.Auth.Request;

namespace Repositories.Implements;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
    public async Task<User> GetByIdAsync(Guid userId)
    {
        List<Expression<Func<User, bool>>> filters = new()
            {
                (user) => user.Id == userId,
                //(user) => user.Status == BaseEntityStatus.Active
            };

        var user = await FirstOrDefaultAsync(filters: filters,
            include: queryable => queryable.Include(u => u.Role!).Include(u => u.Wallets!))
            ?? throw new EntityNotFoundException(MessageConstants.UserMessageConstrant.UserNotFound(userId));
        if (UserStatus.NotVerified == user.Status)
        {
            throw new NotVerifiedAccountException();
        }
        if (UserStatus.Deleted == user.Status)
        {
            throw new BannedAccountException();
        }
        return user;
    }
    public async Task<User?> GetCustomerByQrCodeAsync(string qrCode)
    {
        List<Expression<Func<User, bool>>> filters = new()
            {
                (user) => user.QRCode == qrCode && user.Status == BaseEntityStatus.Active,
                //(user) => user.QrCodeExpiry > TimeUtil.GetCurrentVietNamTime()
            };
        var user = await FirstOrDefaultAsync(
            filters: filters,
            include: queryable => queryable.Include(u => u.Role!).Include(u => u.Wallets!));
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
        var users = await GetListAsync<GetDelivererResponse>(filters: filters);
        return users;
    }
    public async Task<ICollection<GetDelivererResponse>> GetDeliverersAsync()
    {
        List<Expression<Func<User, bool>>> filters = new()
            {
                (user) => RoleName.DELIVERER.ToString().Equals(user.Role!.EnglishName),

            };
        var users = await GetListAsync<GetDelivererResponse>(filters: filters);
        return users;
    }
    public async Task<User> GetManagerByIdAsync(Guid managerId)
    {
        List<Expression<Func<User, bool>>> filters = new()
            {
                (user) => user.Id == managerId,
                //(user) => user.Status == BaseEntityStatus.Active
            };

        var user = await FirstOrDefaultAsync(filters: filters,
            include: queryable => queryable.Include(u => u.Role!).Include(u => u.Kitchen!))
            ?? throw new EntityNotFoundException(MessageConstants.UserMessageConstrant.UserNotFound(managerId));
        if (UserStatus.NotVerified == user.Status)
        {
            throw new NotVerifiedAccountException();
        }
        if (UserStatus.Deleted == user.Status)
        {
            throw new BannedAccountException();
        }
        return user;
    }
    public async Task<User> LoginAsync(LoginRequest loginRequest)
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
        User user = await FirstOrDefaultAsync(filters: whereFilters, include: include) ??
                    throw new InvalidRequestException(MessageConstants.LoginMessageConstrant.InvalidCredentials);
        if (UserStatus.NotVerified == user.Status)
        {
            throw new NotVerifiedAccountException();
        }
        if (UserStatus.Deleted == user.Status)
        {
            throw new BannedAccountException();
        }
        return user;
    }
    public async Task<User> GetUserByIdIncludeWallet(Guid userId)
    {
        List<Expression<Func<User, bool>>> filters = new()
            {
                (user) => user.Id == userId
            };

        var user = await FirstOrDefaultAsync(
            filters: filters,
            include: queryable => queryable.Include(u => u.Role!).Include(u => u.Wallets!))
            ?? throw new EntityNotFoundException(MessageConstants.UserMessageConstrant.UserNotFound(userId));
        if (user.Status == UserStatus.Deleted) throw new BannedAccountException();
        return user;
    }
    public Task<ICollection<GetUserResponse>> GetAllAsync(UserFilterRequest request)
    {
        var filters = new List<Expression<Func<User, bool>>>();
        if (request.RoleId != null && request.RoleId != Guid.Empty)
        {
            filters.Add(u => u.RoleId == request.RoleId);
        }
        var result = GetListAsync<GetUserResponse>(filters: filters);
        return result;
    }
    public Task<GetUserResponse> GetUserResponseByIdAsync(Guid userId)
    {
        var result = FirstOrDefaultAsync<GetUserResponse>(filters: new()
            {
                u => u.Id == userId
            });
        if (result == null) throw new EntityNotFoundException(MessageConstants.UserMessageConstrant.UserNotFound(userId));
        return result!;
    }
    public async Task<User> FindNotVerifiedUserByPhone(string phone)
    {
        return await FirstOrDefaultAsync(
            filters: new List<Expression<Func<User, bool>>>
            {
                    (user) => user.Phone == phone,
                    (user) => user.Status == UserStatus.NotVerified,
            }
        ) ?? throw new EntityNotFoundException(MessageConstants.AuthorizationMessageConstrant.PhoneNotFound);
    }
    public async Task<User> FindUserByPhone(string phone)
    {
        return await FirstOrDefaultAsync(
            filters: new List<Expression<Func<User, bool>>>
            {
                    (user) => user.Phone == phone && user.Status != UserStatus.Deleted
                }
            ) ?? throw new EntityNotFoundException(MessageConstants.AuthorizationMessageConstrant.PhoneNotFound);
    }

}