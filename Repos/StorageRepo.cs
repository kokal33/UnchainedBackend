using Ipfs.CoreApi;
using Ipfs.Http;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using UnchainedBackend.Helpers;

namespace UnchainedBackend.Repos
{

    public interface IStorageRepo
    {
        Task<string> UploadFile(IFormFile audioFile);
        Task<string> UploadText(string text);
        string DecryptFileLink(string cipher, string pass);
    }

    public class StorageRepo : IStorageRepo
    {

        public async Task<string> UploadFile(IFormFile audioFile) {
            IpfsClient client = new();
            
            var fileExtension = Path.GetExtension(audioFile.FileName);
            var filePath = Path.GetTempFileName() + fileExtension;

            using (var stream = File.Create(filePath)) {
                await audioFile.CopyToAsync(stream);
            }

            var upload = await client.FileSystem.AddFileAsync(filePath);
            return upload.Id;
        }

        public async Task<string> UploadText(string text)
        {
            IpfsClient client = new();

            var upload = await client.FileSystem.AddTextAsync(text);
            return upload.Id;
        }


        public string DecryptFileLink(string cipher, string pass) {
            var file = EncryptionHelper.Decrypt(cipher, pass);
            return file;
        }
    }
}
