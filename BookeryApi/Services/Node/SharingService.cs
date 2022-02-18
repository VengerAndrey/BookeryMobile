using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using BookeryApi.Services.Common;
using Domain.Models;

namespace BookeryApi.Services.Node
{
    public class SharingService : BaseService, ISharingService
    {
        public SharingService() : base("Node/Sharing")
        {
            
        }
        public async Task<List<UserNode>> GetSharing(Guid id)
        {
            var response = await _httpClient.GetAsync(Path.Combine("shared-with", id.ToString()));

            if (response.IsSuccessStatusCode)
            {
                var userNodes = await response.Content.ReadAsAsync<List<UserNode>>();
                return userNodes;
            }

            return null;
        }
        
        public async Task<bool> Share(UserNode userNode)
        {
            var response = await _httpClient.PostAsJsonAsync("share", userNode);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Hide(UserNode userNode)
        {
            var response = await _httpClient.PostAsJsonAsync("hide", userNode);

            return response.IsSuccessStatusCode;
        }

        public async Task<Domain.Models.Node> Details(Guid id)
        {
            var response = await _httpClient.GetAsync(Path.Combine("details", id.ToString()));

            if (response.IsSuccessStatusCode)
            {
                var node = await response.Content.ReadAsAsync<Domain.Models.Node>();
                return node;
            }

            return null;
        }
    }
}