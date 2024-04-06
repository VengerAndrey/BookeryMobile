using System;
using BookeryMobile.Data.Enums;

namespace BookeryMobile.Data.DTOs.Node.Input
{
    public record ShareNodeDto(
        Guid NodeId,
        Guid UserId,
        AccessTypeId AccessTypeId
    );
}