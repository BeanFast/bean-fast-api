using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Profiles.Response;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Linq.Expressions;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Statuses;
using Profile = BusinessObjects.Models.Profile;

namespace Repositories.Implements;

public class ProfileRepository : GenericRepository<Profile>, IProfileRepository
{
    public ProfileRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
    public async Task<Profile> GetProfileByIdAsync(int status, Guid id)
    {
        var profile = await FirstOrDefaultAsync(filters: new()
            {
                s => s.Id == id,
                s => s.Status == status
            }) ?? throw new EntityNotFoundException(MessageConstants.ProfileMessageConstrant.ProfileNotFound);
        return profile;
    }
    public async Task<Profile> GetProfileByIdAsync(Guid id)
    {
        var profile = await FirstOrDefaultAsync(filters: new()
            {
                s => s.Id == id
            }) ?? throw new EntityNotFoundException(MessageConstants.ProfileMessageConstrant.ProfileNotFound);
        return profile;
    }

    public async Task<Profile> GetProfileByIdForUpdateProfileAsync(Guid id)
    {
        var profile = await FirstOrDefaultAsync(filters: new()
            {
                s => s.Id == id,
                s => s.Status == BaseEntityStatus.Active
            }, include: i => i.Include(p => p.BMIs!)) ?? throw new EntityNotFoundException(MessageConstants.ProfileMessageConstrant.ProfileNotFound);
        return profile;
    }

    public async Task<ICollection<GetProfileResponse>> GetProfilesByCustomerIdAsync(Guid customerId)
    {
        var profiles = await GetListAsync<GetProfileResponse>(
            filters: new()
            {
                    p => p.UserId == customerId,
                    p => p.Status ==BaseEntityStatus.Active
            }, include: i => i.Include(p => p.School!)
                .Include(p => p.LoyaltyCards!.Where(lc => lc.Status == BaseEntityStatus.Active))
                .Include(p => p.Wallets!
                        .Where(w => w.Status == BaseEntityStatus.Active
                            && WalletType.Points.ToString().Equals(w.Type)
                         )
                    ));
        //foreach (var profile in profiles)
        //{
        //    profile.SchoolName = profile.Schoo
        //}
        return profiles;
    }

    public async Task<Profile> GetByIdAsync(Guid id)
    {
        List<Expression<Func<Profile, bool>>> filters = new()
            {
                (profile) => profile.Id == id && profile.Status != BaseEntityStatus.Deleted
            };
        var profile = await FirstOrDefaultAsync(
            filters: filters, include: queryable => queryable
            .Include(p => p.School!)
            .ThenInclude(s => s.Kitchen)
            .ThenInclude(k => k.Menus!.Where(m => m.Status == BaseEntityStatus.Active))
            .ThenInclude(m => m.MenuDetails!.Where(md => md.Status == BaseEntityStatus.Active)))
            ?? throw new EntityNotFoundException(MessageConstants.ProfileMessageConstrant.ProfileNotFound);
        return profile;
    }


    public async Task<GetProfileResponse> GetProfileResponseByIdAsync(Guid id, User user)
    {
        List<Expression<Func<Profile, bool>>> filters = new()
            {
                (profile) => profile.Id == id
            };
        if (RoleName.CUSTOMER.ToString().Equals(user.Role!.EnglishName))
        {
            filters.Add(p => p.UserId == user.Id);
        }
        filters.Add(p => p.Status != BaseEntityStatus.Deleted);
        var profile = await FirstOrDefaultAsync<GetProfileResponse>(
            filters: filters,
            include: queryable => queryable
                .Include(p => p.School!)
                .Include(p => p.LoyaltyCards!.Where(lc => lc.Status == BaseEntityStatus.Active))
                .Include(p => p.Wallets!
                        .Where(w => w.Status == BaseEntityStatus.Active
                            && WalletType.Points.ToString().Equals(w.Type)
                         )
                    )
            )
            ?? throw new EntityNotFoundException(MessageConstants.ProfileMessageConstrant.ProfileNotFound);
        return profile;
    }

    public async Task<Profile> GetProfileByIdAndCurrentCustomerIdAsync(Guid profileId, Guid customerId)
    {
        var profile = await FirstOrDefaultAsync(filters: new()
            {
                p => p.Id == profileId,
                p => p.UserId == customerId,
                p => p.Status != BaseEntityStatus.Deleted
            }, include: i => i.Include(p => p.Wallets!.Where(w => w.Status == BaseEntityStatus.Active)));
        return profile ?? throw new EntityNotFoundException(MessageConstants.ProfileMessageConstrant.ProfileNotFound);
    }
}