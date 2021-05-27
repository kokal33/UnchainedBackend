using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using UnchainedBackend.Helpers;
using UnchainedBackend.Models;
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
        private readonly IWebHostEnvironment _env;
        private readonly ITracksRepo _tracksRepo;

        public MintingController(ILogger<MintingController> logger, IEthRepo ethRepo, IStorageRepo storageRepo, 
                                 IWebHostEnvironment env, ITracksRepo tracksRepo)
        {
            _logger = logger;
            _ethRepo = ethRepo;
            _storageRepo = storageRepo;
            _env = env;
            _tracksRepo = tracksRepo;
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
        public async Task<IActionResult> Mint([FromBody] MintModel model)
        {
            var baseUrl = HttpContext.Request.Host.Value;
            string magicUrl = null;

            var track = await _tracksRepo.GetTrack(model.TrackId);
            // 1. UPLOAD FILES and get files link on IPFS
            var uploadedFileLink = await _storageRepo.UploadFile(track.FileLocation);
            var uploadedCover = await _storageRepo.UploadFile(track.ImageLocation);
            // 2. ENCRYPT Filename if password present
            if (model.Password != null)
            {
                var encryptPath = EncryptionHelper.Encrypt(uploadedFileLink, model.Password);
                // 3. GET the link to decrypt the audio file
                var magicPartial = Url.Action("DecryptFileLink", new { magicLink = encryptPath });
                magicUrl = "www.unchained-music.com" + magicPartial;
            }

            // 4. Form the METADATA for the NFT and upload to IPFS
            MetadataModel metadataModel = new()
            {
                Name = track.Title,
                Description = track.Description,
                Image = uploadedCover,
                Magic = magicUrl,
                File = magicUrl == null ? uploadedFileLink : null
            };
            var metadata = JsonSerializer.Serialize(metadataModel);
            var uploadedMetadata = await _storageRepo.UploadText(metadata);

            // 4. MINT THE TOKEN WITH THE GIVEN LINK AND METADATA
            MintWithTokenURIModel forMinting = new() { Metadata = uploadedMetadata, To = track.OwnerOfPublicAddress };
            var mint = await _ethRepo.MintWithTokenURI(forMinting);
            if (mint == null) return UnprocessableEntity();

            // Set track as minted in DB
            await _tracksRepo.SetIsMinted(track, true);
            MintReturn result = new()
            {
                TransactionHash = mint.TransactionHash,
                Metadata = metadataModel
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

        [HttpGet]
        public IActionResult DecryptFileLink([FromQuery] string cipher, string pass)
        {
            return Ok(_storageRepo.DecryptFileLink(cipher, pass));
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

        [HttpPost]
        public IActionResult VerifySignature([FromBody] SignatureModel model)
        {
            var result = _ethRepo.VerifySignature(model);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult StartAuction([FromBody] IdModel model)
        {
            AuctionTask auctionTask = new AuctionTask(_logger, model.Id);
            auctionTask.StartAsync(new CancellationToken());
            return Ok();
        }

    }
}
