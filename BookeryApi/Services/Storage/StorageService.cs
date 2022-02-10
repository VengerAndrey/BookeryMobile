using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using BookeryApi.Services.Common;

namespace BookeryApi.Services.Storage
{
    public class StorageService : BaseService, IStorageService
    {
        public StorageService() : base("Storage")
        {
            
        }
        public async Task<bool> Upload(Guid id, Stream data, string filename)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(data), "file", filename);
            var response = await _httpClient.PostAsync(id.ToString(), content);

            return response.IsSuccessStatusCode;
        }

        public async Task<Stream> Download(Guid id)
        {
            var response = await _httpClient.GetAsync(id.ToString());

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                return stream;
            }

            return null;
        }

        public async Task<bool> UploadProfilePhoto(Guid id, Stream data)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(data), "file", id.ToString());
            var response = await _httpClient.PostAsync(Path.Combine("photo", id.ToString()), content);

            return response.IsSuccessStatusCode;
        }

        public async Task<Stream> DownloadProfilePhoto(Guid id)
        {
            var response = await _httpClient.GetAsync(Path.Combine("photo", id.ToString()));

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                return stream;
            }

            return null;
        }
    }
}