// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Domain.Models;
//
// namespace BookeryApi.Services.Storage
// {
//     public interface IShareService
//     {
//         Task<IEnumerable<Share>> GetAll();
//         Task<Share> Get(Guid id);
//         Task<Share> Create(string name);
//         Task<Share> Update(Share share);
//         Task<bool> Delete(Guid id);
//         void SetBearerToken(string accessToken);
//     }
// }