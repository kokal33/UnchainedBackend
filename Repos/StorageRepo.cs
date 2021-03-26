using Ipfs.Http;
using System.IO;
using System.Threading.Tasks;
using UnchainedBackend.Models.PartialModels;

namespace UnchainedBackend.Repos
{

    public interface IStorageRepo
    {
        Task<string> UploadFile(MintModel model);
        Task<Stream> DownloadFile(DownloadFileModel model);
    }

    public class StorageRepo : IStorageRepo
    {

        public async Task<string> UploadFile(MintModel model) {
            IpfsClient client = new IpfsClient();
            
            var fileExtension = Path.GetExtension(model.File.FileName);
            var filePath = Path.GetTempFileName() + fileExtension;

            using (var stream = File.Create(filePath)) {
                await model.File.CopyToAsync(stream);
            }

            var upload = await client.FileSystem.AddFileAsync(filePath);
            return upload.Id;
        }

        public async Task<Stream> DownloadFile(DownloadFileModel model) {
            // Reading files from the system is currently bugged, will need to bypass
            IpfsClient client = new IpfsClient();
            var file = await client.FileSystem.ReadFileAsync(model.FileId);
            return file;
        }
    }
}
