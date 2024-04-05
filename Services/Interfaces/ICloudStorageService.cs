using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICloudStorageService
    {
        public Task<string> UploadFileAsync(Guid id, string folderName, IFormFile file);
        public Task<string> UploadFileAsync(Guid id, string folderName, byte[] bytes, string contentType);

        public Task DeleteFileAsync(Guid id, string folderName);
    }
}
