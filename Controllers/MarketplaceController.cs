using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnchainedBackend.Models;
using UnchainedBackend.Repos;

namespace UnchainedBackend.Controllers
{
    public class MarketplaceController : Controller
    {
        private readonly IMarketplaceRepo _marketplaceRepo;

        public MarketplaceController(IMarketplaceRepo marketplaceRepo)
        {
            _marketplaceRepo = marketplaceRepo;
        }

        [HttpPost]
        public async Task<IActionResult> PostListing([FromBody] Listing model)
        {
            await _marketplaceRepo.PostListing(model);
            return Ok();
        }
    }
}
