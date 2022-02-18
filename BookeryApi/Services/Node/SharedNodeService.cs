using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BookeryApi.Exceptions;
using BookeryApi.Services.Common;
using Domain.Models;

namespace BookeryApi.Services.Node
{
    public class SharedNodeService : BaseService, ISharedNodeService
    {
        public SharedNodeService() : base("Node/Shared")
        {
            
        }
        
        public async Task<List<Domain.Models.Node>> Get(string path)
        {
            var response = await _httpClient.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var nodes = await response.Content.ReadAsAsync<List<Domain.Models.Node>>();
                return nodes;
            }

            return null;
        }

        public async Task<Domain.Models.Node> Create(string path, Domain.Models.Node create)
        {
            var response = await _httpClient.PostAsJsonAsync(path, create);

            if (response.IsSuccessStatusCode)
            {
                var node = await response.Content.ReadAsAsync<Domain.Models.Node>();
                return node;
            }
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new NameConflictException();
            }

            return null;
        }

        public async Task<string> Update(string path, Domain.Models.Node update)
        {
            var response = await _httpClient.PutAsJsonAsync(path, update);

            if (response.IsSuccessStatusCode)
            {
                var newPath = await response.Content.ReadAsStringAsync();
                return newPath;
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new WrongAccessTypeException();
            }

            return null;
        }
    }
}