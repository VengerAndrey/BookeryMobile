using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookeryApi.Services.Common;

namespace BookeryApi.Services.Node
{
    public interface ISharedNodeService : IBaseService, INodeCreateService, INodeUpdateService
    {
        Task<List<Domain.Models.Node>> Get(string path);
        
    }
}