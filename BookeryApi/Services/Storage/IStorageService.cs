using System;
using System.IO;
using System.Threading.Tasks;
using BookeryApi.Services.Common;

namespace BookeryApi.Services.Storage
{
    public interface IStorageService : IBaseService
    {
        Task<bool> Upload(Guid id, Stream data, string filename);
        Task<Stream> Download(Guid id);
        Task<bool> UploadProfilePhoto(Guid id, Stream data);
        Task<Stream> DownloadProfilePhoto(Guid id);
    }
}