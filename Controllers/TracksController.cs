﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UnchainedBackend.Models;
using UnchainedBackend.Models.PartialModels;
using UnchainedBackend.Models.ReturnModels;
using UnchainedBackend.Repos;

namespace UnchainedBackend.Controllers
{
    public class TracksController : Controller
    {
        private readonly ITracksRepo _tracksRepo;
        private readonly IUsersRepo _usersRepo;

        public TracksController(ITracksRepo tracksRepo, IUsersRepo usersRepo) {
            _tracksRepo = tracksRepo;
            _usersRepo = usersRepo;
        }

        [HttpPost]
        public async Task<IActionResult> GetTracks()
        {
            var tracks = await _tracksRepo.GetTracks();
            if (tracks == null) return NoContent();
            return Ok(tracks);
        }
        [HttpPost]
        public async Task<IActionResult> GetTrack([FromBody] IdModel model)
        {
            var track = await _tracksRepo.GetTrack(model.Id);
            if (track == null) return NoContent();
            return Ok(track);
        }

        [HttpPost]
        public async Task<IActionResult> GetMyCollection([FromBody] SignatureModel model)
        {
            var collection = await _tracksRepo.GetMyCollection(model.PublicAddress);
            if (collection == null) return NoContent();
            return Ok(collection);
        }

        [HttpPost]
        public async Task<IActionResult> GetMyCreated([FromBody] SignatureModel model)
        {
            var collection = await _tracksRepo.GetMyCreated(model.PublicAddress);
            if (collection == null) return NoContent();
            return Ok(collection);
        }

        [HttpPost]
        public async Task<IActionResult> PostTrack([FromForm] TrackModel model)
        {
            // Check if the request contains file
            if (model.File == null || model.CoverImage == null) return NoContent();
            var ownerOf = await _usersRepo.GetUser(model.OwnerOfPublicAddress);
            if (ownerOf == null || !ownerOf.Verified)
                return BadRequest();
            var result = await _tracksRepo.PostTrack(model);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTrack([FromBody] int Id)
        {
            var result = await _tracksRepo.DeleteTrack(Id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SetTrackAsSold([FromBody] SetAsSoldModel model)
        {
            var result = await _tracksRepo.SetIsSold(model);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SetNoBidsFinishedAuction([FromBody] IdModel model)
        {
            var result = await _tracksRepo.SetNoBidsFinishedAuction(model.Id);
            return Ok(result);
        }
    }
}
