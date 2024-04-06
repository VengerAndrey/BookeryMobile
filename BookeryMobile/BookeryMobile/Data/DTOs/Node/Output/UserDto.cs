using System;

namespace BookeryMobile.Data.DTOs.Node.Output
{
    public record UserDto(
        Guid Id,
        string Email,
        string FirstName,
        string LastName
    );
}