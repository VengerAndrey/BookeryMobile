using System;

namespace BookeryMobile.Data.DTOs.Node.Input
{
    public record UpdateNodeDto(
        string? Name,
        long? Size,
        Guid ModifiedById
    );
}