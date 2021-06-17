using System;
using System.Threading.Tasks;

namespace BookeryApi.Services.User
{
    public interface IAccessService
    {
        Task<bool> AccessById(Guid id);
        void SetBearerToken(string accessToken);
    }
}