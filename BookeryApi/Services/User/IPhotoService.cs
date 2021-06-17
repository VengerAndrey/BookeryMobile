using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookeryApi.Services.User
{
    public interface IPhotoService
    {
        Task<Stream> Get();
        Task<bool> Set(MultipartFormDataContent content);
        void SetBearerToken(string accessToken);
    }
}