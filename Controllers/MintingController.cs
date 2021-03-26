using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using UnchainedBackend.Models.PartialModels;
using UnchainedBackend.Models.ReturnModels;
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
            // 1. UPLOAD FILE and get the file link
            var uploadedFile = await _storageRepo.UploadFile(model);
            // 2. MINT THE TOKEN WITH THE GIVEN LINK
            var mint = await _ethRepo.MintWithTokenURI(model);
            if (mint == null) return UnprocessableEntity();

            MintReturn result = new MintReturn
            {
                TransactionHash = mint.TransactionHash,
                LinkToFile = uploadedFile
            };
            return Ok(result);
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

        [HttpPost]
        public async Task<IActionResult> TransferTo([FromBody] TransferToModel model)
        {
            var result = await _ethRepo.TransferTo(model);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> OwnerOf([FromBody] OwnerOfModel model)
        {
            var result = await _ethRepo.OwnerOf(model);
            return Ok(result);
        }
    }
}
