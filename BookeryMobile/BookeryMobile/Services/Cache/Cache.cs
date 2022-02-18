using System;
using System.IO;
using System.Threading.Tasks;

namespace BookeryMobile.Services.Cache
{
    public class Cache : ICache
    {
        private readonly string _path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        
        public bool FileExists(string filename)
        {
            return File.Exists(Path.Combine(_path, filename));
        }

        public bool FilesExist()
        {
            return Directory.GetFiles(_path).Length > 0;
        }

        public Stream GetFile(string filename)
        {
            return File.OpenRead(Path.Combine(_path, filename));
        }

        public async Task SaveFile(Stream data, string filename)
        {
            byte[] bytes; 
            using (var memoryStream = new MemoryStream())
            {
                await data.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }
            File.WriteAllBytes(Path.Combine(_path, filename), bytes);
        }

        public void DeleteFile(string filename)
        {
            File.Delete(Path.Combine(_path, filename));
        }

        public void DeleteAllFiles()
        {
            var files = Directory.GetFiles(_path);
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }
    }
}