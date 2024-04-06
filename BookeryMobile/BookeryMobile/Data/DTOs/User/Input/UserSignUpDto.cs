namespace BookeryMobile.Data.DTOs.User.Input
{
    public record UserSignUpDto(
        string Email,
        string Password,
        string FirstName,
        string LastName
    );
}