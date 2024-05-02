using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Settings;

namespace Services.Implements
{
    public class SessionDetailDelivererService : BaseService<SessionDetailDeliverer>, ISessionDetailDelivererService
    {
        public SessionDetailDelivererService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
        }

        public async Task HardDeleteAsync(List<SessionDetailDeliverer> sessionDetailDeliverers)
        {
            await _repository.HardDeleteRangeAsync(sessionDetailDeliverers);
        }

        public async Task InsertListAsync(List<SessionDetailDeliverer> sessionDetailDeliverers)
        {
            foreach (var item in sessionDetailDeliverers)
            {
                await _repository.InsertAsync(item);
            }
            await _unitOfWork.CommitAsync();
        }
    }
}
