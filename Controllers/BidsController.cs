using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnchainedBackend.Models;
using UnchainedBackend.Models.PartialModels;
using UnchainedBackend.Repos;

namespace UnchainedBackend.Controllers
{
    public class BidsController : Controller
    {
        private readonly IBidsRepo _bidsRepo;

        public BidsController(IBidsRepo bidsRepo)
        {
            _bidsRepo = bidsRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetBids()
        {
            var bids = await _bidsRepo.GetBids();
            if (bids == null) return NoContent();
            return Ok(bids);
        }
        [HttpPost]
        public async Task<IActionResult> GetBid([FromBody] SignatureModel model)
        {
            var bid = await _bidsRepo.GetBid(model.PublicAddress);
            if (bid == null) return NoContent();
            return Ok(bid);
        }

        [HttpPost]
        public async Task<IActionResult> Bid([FromBody] Bid bid)
        {
            var result = await _bidsRepo.PostBid(bid);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBid([FromBody] SignatureModel model)
        {
            var result = await _bidsRepo.DeleteBid(model.PublicAddress);
            return Ok(result);
        }
    }
}
