using System.Threading.Tasks;
using BookeryMobile.Data.DTOs.Node.Input;

namespace BookeryMobile.Services.Node.Interfaces
{
    public interface INodeUpdateService
    {
        Task<string?> Update(string path, UpdateNodeDto update);
    }
}