using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using UnchainedBackend.Data;
using UnchainedBackend.Helpers;
using UnchainedBackend.Models;
using UnchainedBackend.Models.PartialModels;
using UnchainedBackend.Models.ReturnModels;

namespace UnchainedBackend.Repos
{
    public interface ITracksRepo
    {
        Task<IEnumerable<TrackReturn>> GetTracks();
        Task<Track> GetTrack(int id);
        Task<int> PostTrack(TrackModel track);
        Task<bool> DeleteTrack(int id);
        Task<bool> UpdateTrack(int trackId, string title, string description, string isMinted, string isAuctioned, string isListed, string isSold);
        Task<bool> SetIsListed(int trackId, bool isListed);
        Task<bool> SetIsSold(int trackId, bool isSold);
        Task<bool> SetIsMinted(Track track, bool isMinted);
        Task<bool> SetTokenId(Track track, int tokenId);
        Task<bool> SetIsAuctioned(int trackId, bool isAuctioned);



    }
    public class TracksRepo : ITracksRepo
    {
        private readonly ApplicationContext _context;
        private IWebHostEnvironment _hostingEnvironment;

        public TracksRepo(ApplicationContext context, IWebHostEnvironment environment)
        {
            _hostingEnvironment = environment;
            _context = context;
        }

        public async Task<IEnumerable<TrackReturn>> GetTracks()
        {
            return await _context.Tracks.Where(w => w.IsSold == false && w.IsMinted==true).Include(t => t.Auction).Include(t=>t.OwnerOf)
                .Select(x => new TrackReturn
            {
                AuctionEnding = x.Auction != null ? x.Auction.Ending : null,
                AuctionId = x.AuctionId,
                IsAuctioned = x.IsAuctioned,
                IsListed = x.IsListed,
                Id = x.Id,
                Description = x.Description,
                FileLocation = x.FileLocation.Split(new[] { "wwwroot" }, StringSplitOptions.None)[1],
                ImageLocation = x.ImageLocation.Split(new[] { "wwwroot" }, StringSplitOptions.None)[1],
                OwnerOfProfilePic = x.OwnerOf.ProfilePic,
                OwnerOfPublicAddress = x.OwnerOfPublicAddress,
                Title = x.Title,
                TokenId = x.TokenId,
                Price = x.Auction != null ? x.Auction.Price : null
            }).ToListAsync();
        }

        public async Task<Track> GetTrack(int id)
        {
            var track = await _context.Tracks.Include(x => x.OwnerOf).Include(x=>x.Auction).FirstOrDefaultAsync(t => t.Id == id);
            return track;
        }       

        public async Task<int> PostTrack(TrackModel model)
        {
            // Write the track to storage
            string fileContent = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", model.OwnerOfPublicAddress, "Tracks");
            string filePath = Path.Combine(fileContent, model.File.FileName);
            (new FileInfo(filePath)).Directory.Create();
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                await model.File.CopyToAsync(fileStream);
            // Write the track image to store
            //TODO: check here for 
            string trackImageContent = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", model.OwnerOfPublicAddress, "TrackImages");
            string imagePath = Path.Combine(trackImageContent, model.File.FileName);
            (new FileInfo(imagePath)).Directory.Create();
            using (Stream fileStream = new FileStream(imagePath, FileMode.Create))
                await model.CoverImage.CopyToAsync(fileStream);
            
            var newTrack = ModelsHelper.MapModelToTrack(model);
            newTrack.ImageLocation = imagePath;
            newTrack.FileLocation = filePath;
            newTrack.Timestamp = DateTime.Now;

                _context.Tracks.Add(newTrack);
            await _context.SaveChangesAsync();

            return newTrack.Id;
        }

        public async Task<bool> DeleteTrack(int id)
        {
            var track = await _context.Tracks.FindAsync(id);

            _context.Tracks.Remove(track);
            await _context.SaveChangesAsync();
            return true;
        }

        // Bool values will be represented by strings for exact management
        public async Task<bool> UpdateTrack(int trackId, string title, string description, string isMinted, 
                                            string isAuctioned, string isListed, string isSold)
        {
            var track = await _context.Tracks.FindAsync(trackId);
            _context.Attach(track);
            if (title != null)
                track.Title = title;
            if (description != null)
                track.Description = description;
            if (isMinted != null) {
                if (isMinted == "true")
                    track.IsMinted = true;
                if (isMinted == "false")
                    track.IsMinted = false;
            }
            if (isAuctioned != null)
            {
                if (isAuctioned == "true")
                    track.IsAuctioned = true;
                if (isAuctioned == "false")
                    track.IsAuctioned = false;
            }
            if (isListed != null)
            {
                if (isListed == "true")
                    track.IsListed = true;
                if (isListed == "false")
                    track.IsListed = false;
            }
            if (isSold != null)
            {
                if (isSold == "true")
                    track.IsSold = true;
                if (isSold == "false")
                    track.IsSold = false;
            }
            _context.Entry(track).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetIsListed(int trackId, bool isListed) 
        {
            Track track = new Track { Id = trackId, IsListed = isListed };
            _context.Entry(track).Property(x => x.IsListed).IsModified = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetIsMinted(Track track, bool isMinted)
        {
            track.IsMinted = true;
            _context.Entry(track).Property(x => x.IsMinted).IsModified = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetIsSold(int trackId, bool isSold)
        {
            Track track = new Track { Id = trackId, IsSold = isSold };
            _context.Entry(track).Property(x => x.IsSold).IsModified = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetIsAuctioned(int trackId, bool isAuctioned)
        {
            Track track = new Track { Id = trackId, IsAuctioned = isAuctioned };
            _context.Entry(track).Property(x => x.IsAuctioned).IsModified = true;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> SetTokenId(Track track, int tokenId)
        {
            track.TokenId = tokenId;
            _context.Entry(track).Property(x => x.TokenId).IsModified = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
