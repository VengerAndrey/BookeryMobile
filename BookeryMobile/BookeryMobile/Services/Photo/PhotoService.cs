using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using BookeryMobile.Services.Common;

namespace BookeryMobile.Services.Photo
{
    public class PhotoService : BaseService, IPhotoService
    {
        public PhotoService() : base("Photo")
        {
            
        }
        public async Task<bool> UploadProfilePhoto(Stream data)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(data), "file", "profile");
            var response = await HttpClient.PostAsync("Profile", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<Stream?> DownloadProfilePhoto()
        {
            var response = await HttpClient.GetAsync("Profile");

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                return stream;
            }

            return null;
        }
        
    }
}