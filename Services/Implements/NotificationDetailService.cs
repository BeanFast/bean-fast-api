﻿using AutoMapper;
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
    public class NotificationDetailService : BaseService<NotificationDetail>, INotificationDetailService
    {
        public NotificationDetailService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
        }

        public Task MarkedAsReadAsync(Guid notificationId, Guid userId)
        {
            return Task.CompletedTask;
        }
    }
}
