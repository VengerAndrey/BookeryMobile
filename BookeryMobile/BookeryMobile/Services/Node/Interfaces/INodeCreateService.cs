using System.Threading.Tasks;
using BookeryMobile.Data.DTOs.Node.Input;
using BookeryMobile.Data.DTOs.Node.Output;

namespace BookeryMobile.Services.Node.Interfaces
{
    public interface INodeCreateService
    {
        Task<NodeDto?> Create(string path, CreateNodeDto create);
    }
}