using System.Threading.Tasks;

namespace BookeryApi.Services.Node
{
    public interface INodeCreateService
    {
        Task<Domain.Models.Node> Create(string path, Domain.Models.Node create);
    }
}