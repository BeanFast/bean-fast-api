﻿using Google;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Extensions.Msal;
using Services.Interfaces;
using System;
using System.Collections;
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
        private readonly FirebaseSettings _firebaseSetting;

        public FirebaseCloudStorageService(StorageClient storageClient, IOptions<AppSettings> settings)
        {
            _storageClient = storageClient;
            _appSettings = settings.Value;
            _firebaseSetting = _appSettings.Firebase;

        }
        public async Task<string> UploadFileAsync(Guid id, string folderName, IFormFile file)
        {
            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                FirebaseSettings firebaseSetting = _appSettings.Firebase;
                await _storageClient.UploadObjectAsync(firebaseSetting.StorageBucket, $"{folderName}/{id}", file.ContentType, stream);
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
        public async Task DeleteFileAsync(Guid id, string folderName)
        {
            try
            {
                await _storageClient.DeleteObjectAsync(
                    _firebaseSetting.StorageBucket,
                    $"{folderName}/{id}",
                    null,
                    CancellationToken.None
                    );
                
            }
            catch (GoogleApiException ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }

        public async Task<string> UploadFileAsync(Guid id, string folderName, byte[] bytes, string contentType)
        {
            FirebaseSettings firebaseSetting = _appSettings.Firebase;
            Stream stream = new MemoryStream(bytes);

            await _storageClient.UploadObjectAsync(firebaseSetting.StorageBucket, $"{folderName}/{id}", contentType, stream);
            var baseURL = firebaseSetting.BaseUrl;
            var filePath = $"{folderName}%2F{id}";
            var url = $"{baseURL}/{firebaseSetting.StorageBucket}/o/{filePath}?alt=media";
            return url;
        }
    }
}
