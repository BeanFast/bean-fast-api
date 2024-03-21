using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Wallet.Response;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Settings;
using Utilities.Statuses;

namespace Services.Implements
{
    public class WalletService : BaseService<Wallet>, IWalletService
    {
        public WalletService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
        }

        public async Task<ICollection<GetWalletByCurrentCustomerAndProfileResponse>> GetWalletByCurrentCustomerAndProfileAsync(Guid customerId, Guid profileId)
        {
            List<Expression<Func<Wallet, bool>>> filters = new()
            {
                p => p.UserId == customerId,
                p => p.ProfileId == profileId
            };
            var wallets = await _repository.GetListAsync<GetWalletByCurrentCustomerAndProfileResponse>(
                status: BaseEntityStatus.Active,
                filters: filters);
            return wallets;
        }
    }
}
