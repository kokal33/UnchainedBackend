using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnchainedBackend.Models;
using UnchainedBackend.Models.PartialModels;
using UnchainedBackend.Repos;

namespace UnchainedBackend.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IArtistsRepo _artistsRepo;

        public ArtistsController(IArtistsRepo artistsRepo)
        {
            _artistsRepo = artistsRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetArtists()
        {
            var artists = await _artistsRepo.GetArtists();
            if (artists == null) return NoContent();
            return Ok(artists);
        }
        [HttpPost]
        public async Task<IActionResult> GetArtist([FromBody] SignatureModel model)
        {
            var artist = await _artistsRepo.GetArtist(model.PublicAddress);
            if (artist == null) return NoContent();
            return Ok(artist);
        }

        [HttpPost]
        public async Task<IActionResult> PostArtistAsync([FromBody] Artist artist)
        {
            var result = await _artistsRepo.PostArtist(artist);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteArtist([FromBody] SignatureModel model)
        {
            var result = await _artistsRepo.DeleteArtist(model.PublicAddress);
            return Ok(result);
        }
    }
}
