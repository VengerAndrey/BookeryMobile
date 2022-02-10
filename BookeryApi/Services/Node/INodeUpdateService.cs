using System.Threading.Tasks;

namespace BookeryApi.Services.Node
{
    public interface INodeUpdateService
    {
        Task<string> Update(string path, Domain.Models.Node update);
    }
}