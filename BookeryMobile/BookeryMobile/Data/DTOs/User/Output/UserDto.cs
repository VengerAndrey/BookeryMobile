using System;

namespace BookeryMobile.Data.DTOs.User.Output
{
    public record UserDto(
        Guid Id,
        string Email,
        string FirstName,
        string LastName
    );
}