using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Notification.Request;
using DataTransferObjects.Models.Notification.Response;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Repositories.Interfaces;
using Services.Interfaces;
using Utilities.Settings;
using Utilities.Statuses;
using Utilities.Utils;

namespace Services.Implements
{
    public class NotificationService : BaseService<BusinessObjects.Models.Notification>, INotificationService
    {
        private readonly IUserService _userService;
        private readonly INotificationRepository _repository;
        public NotificationService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IUserService userService, INotificationRepository repository) : base(unitOfWork, mapper, appSettings)
        {
            _userService = userService;
            _repository = repository;
        }

        public async Task<IPaginable<GetNotificationResponse>> GetNotificationPageByCurrentUser(PaginationRequest paginationRequest, User user)
        {
            var notificationsPage = await _repository.GetPageAsync(paginationRequest, filters: new()
            {
                n => n.NotificationDetails.Any(nd => nd.UserId == user.Id),
                n => n.Status != BaseEntityStatus.Deleted
            }, include: i => i.Include(n => n.NotificationDetails.Where(nd => nd.UserId == user.Id)));
            Paginate<GetNotificationResponse> result = new Paginate<GetNotificationResponse>
            {
                Page = notificationsPage.Page,
                TotalPages = notificationsPage.TotalPages,
                Items = _mapper.Map<List<GetNotificationResponse>>(notificationsPage.Items),
                Size = notificationsPage.Size,
                Total = notificationsPage.Total
            };
            

            return result;
        }

        public async Task MarkAsReadNotificationAsync(MarkAsReadNotificationRequest request, User user)
        {
            List<Guid> notFoundNotificationIds = new List<Guid>();
            foreach (var notificationId in request.NotificationIds)
            {

                var notification = await _repository.FirstOrDefaultAsync(filters: new()
                {
                    n => n.Id == notificationId && n.Status == BaseEntityStatus.Active
                }, include: i => i.Include(n => n.NotificationDetails.Where(nd => nd.Status == NotificationDetailStatus.Unread && nd.UserId == user.Id)));
                if (notification == null)
                {
                    notFoundNotificationIds.Add(notificationId);
                }
                else
                {
                    if (notification.NotificationDetails.Any())
                    {
                        foreach (var nd in notification.NotificationDetails)
                        {
                            nd.ReadDate = TimeUtil.GetCurrentVietNamTime();
                            nd.Status = NotificationDetailStatus.Read;
                        }
                        await _repository.UpdateAsync(notification);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
        }

        public async Task<BatchResponse> SendNotificationAsync(CreateNotificationRequest request)
        {
            List<string> deviceTokens = new();
            var notification = _mapper.Map<BusinessObjects.Models.Notification>(request);
            notification.Id = Guid.NewGuid();
            notification.Status = BaseEntityStatus.Active;
            foreach (var notificationDetail in notification.NotificationDetails)
            {
                var user = await _userService.GetByIdAsync(notificationDetail.UserId);
                notificationDetail.SendDate = TimeUtil.GetCurrentVietNamTime();
                notificationDetail.Status = NotificationDetailStatus.Unread;
                notificationDetail.Id = Guid.NewGuid();
                if (user.DeviceToken != null)
                {
                    deviceTokens.Add(user.DeviceToken);
                }
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
                Tokens = deviceTokens
            };
            var app = FirebaseApp.DefaultInstance;

            if (app == null)
            {
                GoogleCredential credential;
                var credentialJsonFileName = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");

                credential = GoogleCredential.FromFile(credentialJsonFileName);

                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = credential,
                });
            }
            FirebaseMessaging messaging = FirebaseMessaging.GetMessaging(app);
            var response = await messaging.SendMulticastAsync(message);
            //var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            await _repository.InsertAsync(notification);
            await _unitOfWork.CommitAsync();
            return response;
        }
    }
}
