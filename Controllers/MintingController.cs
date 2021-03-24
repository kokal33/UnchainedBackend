using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using UnchainedBackend.Models.PartialModels;
using UnchainedBackend.Repos;

namespace UnchainedBackend.Controllers
{
    public class MintingController : Controller
    {
        private readonly ILogger<MintingController> _logger;
        private readonly IEthRepo _ethRepo;
        private readonly IStorageRepo _storageRepo;


        public MintingController(ILogger<MintingController> logger, IEthRepo ethRepo, IStorageRepo storageRepo)
        {
            _logger = logger;
            _ethRepo = ethRepo;
            _storageRepo = storageRepo;
        }

        /// <summary>
        /// Deploys the contract of type UnchainedToken
        /// with the given parameters
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> DeployAndGetContract([FromBody] DeployContractModel model)
        {
            var contractAddress = await _ethRepo.DeployAndCall(model);
            return Ok(contractAddress);
        }

        [HttpPost]
        public async Task<IActionResult> GetAccountBalance([FromBody] GetAccountBalanceModel model)
        {
            var balance = await _ethRepo.GetBalanceOfAddress(model);
            return Ok(balance);
        }

        [HttpPost]
        public async Task<IActionResult> Mint([FromForm] MintModel model)
        {
            // Check if the request contains file
            if (model.File == null) return new UnsupportedMediaTypeResult();

            var uploadedFile = await _storageRepo.UploadFile(model);
            var balance = await _ethRepo.MintWithTokenURI(model);
            if (balance == null) return UnprocessableEntity();

            return Ok(balance.TransactionHash);
        }

        public async Task<IActionResult> GetContractSupply()
        {
            var supply = await _ethRepo.GetContractSupply();
            return Ok(supply);
        }

        public async Task<IActionResult> GetTokenURI([FromBody] GetTokenURIModel model)
        {
            var tokenURI = await _ethRepo.GetTokenURI(model);
            return Ok(tokenURI);
        }

        [HttpPost]
        public async Task<FileResult> DownloadFile([FromBody] DownloadFileModel model)
        {
            var file = await _storageRepo.DownloadFile(model);
            return File(file, "audio/mp3");
        }
    }
}
