using System;
using System.IO;
using System.Threading.Tasks;
using BookeryMobile.Services.Common;

namespace BookeryMobile.Services.Storage
{
    public interface IStorageService : IBaseService
    {
        Task<bool> Upload(Guid id, Stream data, string filename);
        Task<Stream?> Download(Guid id);
    }
}