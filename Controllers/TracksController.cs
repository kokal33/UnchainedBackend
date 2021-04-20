using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnchainedBackend.Models;
using UnchainedBackend.Repos;

namespace UnchainedBackend.Controllers
{
    public class TracksController : Controller
    {
        private readonly ITracksRepo _tracksRepo;

        public TracksController(ITracksRepo tracksRepo) {
            _tracksRepo = tracksRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetTracks()
        {
            var tracks = await _tracksRepo.GetTracks();
            if (tracks == null) return NoContent();
            return Ok(tracks);
        }
        [HttpPost]
        public async Task<IActionResult> GetTrack([FromBody] int Id)
        {
            var track = await _tracksRepo.GetTrack(Id);
            if (track == null) return NoContent();
            return Ok(track);
        }

        [HttpPost]
        public async Task<IActionResult> PostTrack([FromBody] Track track)
        {
            var result = await _tracksRepo.PostTrack(track);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTrack([FromBody] int Id)
        {
            var result = await _tracksRepo.DeleteTrack(Id);
            return Ok(result);
        }
    }
}
