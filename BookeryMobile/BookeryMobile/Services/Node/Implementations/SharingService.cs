using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using BookeryMobile.Data.DTOs.Node.Input;
using BookeryMobile.Data.DTOs.Node.Output;
using BookeryMobile.Services.Common;
using BookeryMobile.Services.Node.Interfaces;

namespace BookeryMobile.Services.Node.Implementations
{
    public class SharingService : BaseService, ISharingService
    {
        public SharingService() : base("Node/Sharing")
        {
            
        }
        public async Task<List<UserDto>?> GetSharing(Guid id)
        {
            var response = await HttpClient.GetAsync(Path.Combine("SharedWith", id.ToString()));

            if (response.IsSuccessStatusCode)
            {
                var userNodes = await response.Content.ReadAsAsync<List<UserDto>>();
                return userNodes;
            }

            return null;
        }
        
        public async Task<bool> Share(ShareNodeDto userNode)
        {
            var response = await HttpClient.PostAsJsonAsync("Share", userNode);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Hide(HideNodeDto userNode)
        {
            var response = await HttpClient.PostAsJsonAsync("Hide", userNode);

            return response.IsSuccessStatusCode;
        }

        public async Task<NodeDto?> Details(Guid id)
        {
            var response = await HttpClient.GetAsync(Path.Combine("Details", id.ToString()));

            if (response.IsSuccessStatusCode)
            {
                var node = await response.Content.ReadAsAsync<NodeDto>();
                return node;
            }

            return null;
        }
    }
}