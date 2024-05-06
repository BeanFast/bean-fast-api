using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class KitchenRepository : GenericRepository<Kitchen>, IKitchenRepository
    {
        public KitchenRepository(BeanFastContext context, IMapper mapper) : base(context, mapper) 
        {

        }
    }

}
