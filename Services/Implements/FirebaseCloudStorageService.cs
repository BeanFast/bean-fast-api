using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Extensions.Msal;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Utilities.Settings;

namespace Services.Implements
{
    public class FirebaseCloudStorageService : ICloudStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly AppSettings _appSettings;

        public FirebaseCloudStorageService(StorageClient storageClient, IOptions<AppSettings> settings)
        {
            _storageClient = storageClient;
            _appSettings = settings.Value;
        }
        public async Task<string> UploadFileAsync(Guid id, string folderName, string contentType, IFormFile file)
        {
            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                FirebaseSettings firebaseSetting = _appSettings.Firebase;
                await _storageClient.UploadObjectAsync(firebaseSetting.StorageBucket, $"{folderName}/{id}", contentType, stream);
                var baseURL = firebaseSetting.BaseUrl;
                var filePath = $"{folderName}%2F{id}";
                var url = $"{baseURL}/{firebaseSetting.StorageBucket}/o/{filePath}?alt=media";
                return url;
            }
            catch
            {
                throw;
            }
        }
    }
}
