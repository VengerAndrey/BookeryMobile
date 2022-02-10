using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookeryApi.Services.Common;

namespace BookeryApi.Services.Node
{
    public interface ISharedNodeService : IBaseService, INodeCreateService, INodeUpdateService
    {
        Task<List<Domain.Models.Node>> Get(string path);
        Task<List<Domain.Models.UserNode>> GetSharing(Guid id);
        Task<bool> Share(Domain.Models.UserNode userNode);
        Task<bool> Hide(Domain.Models.UserNode userNode);
        Task<Domain.Models.Node> Details(Guid id);
    }
}