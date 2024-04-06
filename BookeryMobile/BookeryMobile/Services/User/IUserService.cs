using System;
using System.Threading.Tasks;
using BookeryMobile.Data.DTOs.User.Input;
using BookeryMobile.Data.DTOs.User.Output;
using BookeryMobile.Services.Common;

namespace BookeryMobile.Services.User
{
    public interface IUserService : IBaseService
    {
        Task SignUp(UserSignUpDto userSignUpDto);
        Task<UserDto> Get();
        Task<UserDto> GetByEmail(string email);
        Task<UserDto> GetById(Guid id);
    }
}