using System.IO;
using System.Threading.Tasks;

namespace BookeryMobile.Services.Cache
{
    public interface ICache
    {
        bool FileExists(string filename);
        bool FilesExist();
        Stream GetFile(string filename);
        Task SaveFile(Stream data, string filename);
        void DeleteFile(string filename);
        void DeleteAllFiles();
    }
}