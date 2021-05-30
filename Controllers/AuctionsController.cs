using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnchainedBackend.Models;
using UnchainedBackend.Models.PartialModels;
using UnchainedBackend.Repos;

namespace UnchainedBackend.Controllers
{
    public class AuctionsController : Controller
    {
        private readonly IAuctionRepo _auctionRepo;

        public AuctionsController(IAuctionRepo auctionRepo)
        {
            _auctionRepo = auctionRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveAuctions()
        {
            var auctions = await _auctionRepo.GetActiveAuctions();
            if (auctions == null) return NoContent();
            return Ok(auctions);
        }

        public async Task<IActionResult> PostAuction([FromBody] AuctionModel model)
        {
            Auction auction = new()
            {
                Ending = model.Ending,
                IsEnded = false,
                Started = model.Started,
                TrackId = model.TrackId
            };
            var posted = await _auctionRepo.SetAuction(auction);
            return Ok(posted);
        }
    }
}
