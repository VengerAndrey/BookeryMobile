using System;
using System.IO;
using System.Threading.Tasks;
using BookeryApi.Services.Common;

namespace BookeryApi.Services.Photo
{
    public interface IPhotoService : IBaseService
    {
        Task<bool> UploadProfilePhoto(Stream data);
        Task<Stream> DownloadProfilePhoto();
    }
}