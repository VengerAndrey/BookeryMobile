using System;

namespace BookeryMobile.Data.DTOs.Node.Input
{
    public record HideNodeDto(
        Guid NodeId,
        Guid UserId
    );
}