namespace BookeryMobile.Data.DTOs.Node.Input
{
    public record CreateNodeDto(
        string Name,
        bool IsDirectory
    );
}