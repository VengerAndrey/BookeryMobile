using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Domain.Models;

namespace BookeryApi.Services.Storage
{
    public class ItemService : IItemService
    {
        private readonly HttpClient _httpClient;

        public ItemService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://10.0.2.2:42396/api/Item/");
        }

        public async Task<Item> GetItem(string path)
        {
            var response = await _httpClient.GetAsync($"item/{path}");

            if (response.IsSuccessStatusCode)
            {
                var item = await response.Content.ReadAsAsync<Item>();

                return item;
            }

            return null;
        }

        public async Task<List<Item>> GetSubItems(string path)
        {
            var response = await _httpClient.GetAsync($"sub-items/{path}");

            if (response.IsSuccessStatusCode)
            {
                var items = await response.Content.ReadAsAsync<List<Item>>();

                return items;
            }

            return null;
        }

        public async Task<Item> CreateDirectory(string path)
        {
            var response = await _httpClient.PostAsJsonAsync($"create-directory/{path}", "");

            if (response.IsSuccessStatusCode)
            {
                var createdDirectory = await response.Content.ReadAsAsync<Item>();

                return createdDirectory;
            }

            return null;
        }

        public async Task<Item> RenameFile(string path, string name)
        {
            var response = await _httpClient.PostAsJsonAsync($"rename-file/{path}", name);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<Item>();

                return result;
            }

            return null;
        }

        public async Task<Item> UploadFile(string path, MultipartFormDataContent content)
        {
            var response = await _httpClient.PostAsync($"upload-file/{path}", content);

            if (response.IsSuccessStatusCode)
            {
                var item = await response.Content.ReadAsAsync<Item>();

                return item;
            }

            return null;
        }

        public async Task<Stream> DownloadFile(string path)
        {
            var response = await _httpClient.GetAsync($"download-file/{path}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStreamAsync();
            }

            return null;
        }

        public async Task<bool> Delete(string path)
        {
            var response = await _httpClient.DeleteAsync($"delete/{path}");

            return await response.Content.ReadAsAsync<bool>();
        }

        public void SetBearerToken(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}