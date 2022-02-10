using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using BookeryApi.Services.Common;
using Domain.Models;

namespace BookeryApi.Services.Node
{
    public class SharedNodeService : BaseService, ISharedNodeService
    {
        public SharedNodeService() : base("Node")
        {
            
        }
        
        public async Task<List<Domain.Models.Node>> Get(string path)
        {
            var response = await _httpClient.GetAsync(Path.Combine("shared", path));

            if (response.IsSuccessStatusCode)
            {
                var nodes = await response.Content.ReadAsAsync<List<Domain.Models.Node>>();
                return nodes;
            }

            return null;
        }

        public async Task<List<UserNode>> GetSharing(Guid id)
        {
            var response = await _httpClient.GetAsync(Path.Combine("sharing", id.ToString()));

            if (response.IsSuccessStatusCode)
            {
                var userNodes = await response.Content.ReadAsAsync<List<UserNode>>();
                return userNodes;
            }

            return null;
        }

        public async Task<Domain.Models.Node> Create(string path, Domain.Models.Node create)
        {
            var response = await _httpClient.PostAsJsonAsync(Path.Combine("create", path), create);

            if (response.IsSuccessStatusCode)
            {
                var node = await response.Content.ReadAsAsync<Domain.Models.Node>();
                return node;
            }

            return null;
        }

        public async Task<string> Update(string path, Domain.Models.Node update)
        {
            var response = await _httpClient.PutAsJsonAsync(Path.Combine("update", path), update);

            if (response.IsSuccessStatusCode)
            {
                var newPath = await response.Content.ReadAsStringAsync();
                return newPath;
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