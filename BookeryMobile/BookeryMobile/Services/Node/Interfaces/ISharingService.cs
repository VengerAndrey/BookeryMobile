using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookeryMobile.Data.DTOs.Node.Input;
using BookeryMobile.Data.DTOs.Node.Output;
using BookeryMobile.Services.Common;

namespace BookeryMobile.Services.Node.Interfaces
{
    public interface ISharingService : IBaseService
    {
        Task<List<UserDto>?> GetSharing(Guid id);
        Task<bool> Share(ShareNodeDto userNode);
        Task<bool> Hide(HideNodeDto userNode);
        Task<NodeDto?> Details(Guid id);
    }
}