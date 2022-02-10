using System.Threading.Tasks;
using BookeryApi.Services.Common;

namespace BookeryApi.Services.User
{
    public interface IUserService : IBaseService
    {
        Task<Domain.Models.User> Get();
    }
}