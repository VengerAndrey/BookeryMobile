using System.Collections.Generic;
using System.Threading.Tasks;
using BookeryMobile.Data.DTOs.Node.Output;
using BookeryMobile.Services.Common;

namespace BookeryMobile.Services.Node.Interfaces
{
    public interface ISharedNodeService : IBaseService, INodeCreateService, INodeUpdateService
    {
        Task<List<NodeDto>?> Get(string path);
    }
}