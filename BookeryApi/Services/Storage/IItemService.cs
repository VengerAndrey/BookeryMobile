// using System.Collections.Generic;
// using System.IO;
// using System.Net.Http;
// using System.Threading.Tasks;
// using Domain.Models;
//
// namespace BookeryApi.Services.Storage
// {
//     public interface IItemService
//     {
//         Task<Item> GetItem(string path);
//         Task<List<Item>> GetSubItems(string path);
//         Task<Item> CreateDirectory(string path);
//         Task<Item> RenameFile(string path, string name);
//         Task<Item> UploadFile(string path, MultipartFormDataContent content);
//         Task<Stream> DownloadFile(string path);
//         Task<bool> Delete(string path);
//         void SetBearerToken(string accessToken);
//     }
// }