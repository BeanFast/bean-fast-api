using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.CardType.Request;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Settings;
using Utilities.Statuses;

namespace Services.Implements
{
    public class CardTypeService : BaseService<CardType>, ICardTypeService
    {
        public CardTypeService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
        }

        public async Task<ICollection<GetCardTypeResponse>> GetAllAsync()
        {
            var cardTypes = await _repository.GetListAsync(
                BaseEntityStatus.Active,
                selector: c => _mapper.Map<GetCardTypeResponse>(c)
                );
            return cardTypes;
        }
    }
}
