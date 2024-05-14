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
        private readonly ISessionDetailDelivererRepository _delivererRepository;
        public SessionDetailDelivererService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, ISessionDetailDelivererRepository delivererRepository) : base(unitOfWork, mapper, appSettings)
        {
            _delivererRepository = delivererRepository;
        }

        public async Task HardDeleteAsync(List<SessionDetailDeliverer> sessionDetailDeliverers)
        {
            await _delivererRepository.HardDeleteRangeAsync(sessionDetailDeliverers);
        }

        public async Task InsertListAsync(List<SessionDetailDeliverer> sessionDetailDeliverers)
        {
            foreach (var item in sessionDetailDeliverers)
            {
                await _delivererRepository.InsertAsync(item);
            }
            await _unitOfWork.CommitAsync();
        }
        public async Task<ICollection<SessionDetailDeliverer>> GetBySessionDetailId(Guid sessionDetailId)
        {
            return await _delivererRepository.GetBySessionDetailId(sessionDetailId);
        }
    }
}
