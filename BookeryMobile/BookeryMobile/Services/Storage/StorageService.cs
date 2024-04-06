using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using BookeryMobile.Services.Common;

namespace BookeryMobile.Services.Storage
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
            var response = await HttpClient.PostAsync(id.ToString(), content);

            return response.IsSuccessStatusCode;
        }

        public async Task<Stream?> Download(Guid id)
        {
            var response = await HttpClient.GetAsync(id.ToString());

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                return stream;
            }

            return null;
        }
    }
}