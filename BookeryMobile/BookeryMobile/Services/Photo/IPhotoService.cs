using System.IO;
using System.Threading.Tasks;
using BookeryMobile.Services.Common;

namespace BookeryMobile.Services.Photo
{
    public interface IPhotoService : IBaseService
    {
        Task<bool> UploadProfilePhoto(Stream data);
        Task<Stream?> DownloadProfilePhoto();
    }
}