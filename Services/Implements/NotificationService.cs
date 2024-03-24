using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Notification.Request;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
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
    public class NotificationService : BaseService<BusinessObjects.Models.Notification>, INotificationService
    {
        private readonly IUserService _userService;
        public NotificationService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IUserService userService) : base(unitOfWork, mapper, appSettings)
        {
            _userService = userService;
        }

        public async Task SendNotificationAsync(CreateNotificationRequest request)
        {
            List<string> deviceTokens = new();
            var notification = _mapper.Map<BusinessObjects.Models.Notification>(request);
            foreach (var notificationDetail in notification.NotificationDetails)
            {
                await _userService.GetByIdAsync(notificationDetail.UserId);
                notificationDetail.SendDate = DateTime.UtcNow;
                notificationDetail.Status = BaseEntityStatus.Active;
                //notificationDetail.
            }
            var messageData = new Dictionary<string, string>
            {
                { "link", request.Link ?? "" },
            };
            var message = new MulticastMessage()
            {
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = request.Title,
                    Body = request.Body
                },
                Data = messageData,
                Tokens = new List<string>
                {
                    request.DeviceToken
                }
            };
            var app = FirebaseApp.DefaultInstance;
            if (FirebaseApp.DefaultInstance == null)
            {
                GoogleCredential credential;
                var credentialJsonFileName = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");

                credential = GoogleCredential.FromFile(credentialJsonFileName);

                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = credential
                });
            }
            FirebaseMessaging messaging = FirebaseMessaging.GetMessaging(app);
            
            var response = await messaging.SendMulticastAsync(message);
            await _repository.InsertAsync(notification);
            await _unitOfWork.CommitAsync();
            await Console.Out.WriteLineAsync(response.ToString());
        }
    }
}
