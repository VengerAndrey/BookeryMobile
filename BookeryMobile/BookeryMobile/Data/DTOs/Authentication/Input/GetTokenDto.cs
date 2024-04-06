namespace BookeryMobile.Data.DTOs.Authentication.Input
{
    public record GetTokenDto(
        string Email,
        string Password
    );
}