using System;

namespace BookeryMobile.Data.DTOs.Authentication.Output
{
    public record TokenDto(
        string Email,
        string AccessToken,
        string RefreshToken,
        DateTime ExpireAt
    );
}