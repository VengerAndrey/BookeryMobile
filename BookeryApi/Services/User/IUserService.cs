using System.Threading.Tasks;

namespace BookeryApi.Services.User
{
    public interface IUserService
    {
        Task<Domain.Models.User> Get();
        void SetBearerToken(string accessToken);
    }
}