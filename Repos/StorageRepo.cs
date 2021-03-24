using Ipfs.Engine;
using Ipfs.Http;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using UnchainedBackend.Models.PartialModels;

namespace UnchainedBackend.Repos
{

    public interface IStorageRepo
    {
        Task<bool> UploadFile(MintModel model);
        Task<Stream> DownloadFile(DownloadFileModel model);
    }

    public class StorageRepo : IStorageRepo
    {

        public async Task<bool> UploadFile(MintModel model) {
            IpfsClient client = new IpfsClient();
            
            var fileExtension = Path.GetExtension(model.File.FileName);
            var filePath = Path.GetTempFileName();

            using (var stream = File.Create(filePath)) {
                await model.File.CopyToAsync(stream);
            }

            var kokal = await client.FileSystem.AddFileAsync(filePath);
            return true;
        }

        public async Task<Stream> DownloadFile(DownloadFileModel model) {
            // Reading files from the system is currently bugged, will need to bypass
            IpfsClient client = new IpfsClient();
            var file = await client.FileSystem.ReadFileAsync(model.FileId);
            return file;
        }
    }
}
