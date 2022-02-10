using System.Collections.Generic;
using System.Threading.Tasks;
using BookeryApi.Services.Common;

namespace BookeryApi.Services.Node
{
    public interface IPrivateNodeService : IBaseService, INodeCreateService, INodeUpdateService
    {
        Task<List<Domain.Models.Node>> Get(string path);
        Task<bool> Delete(string path);
    }
}