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
        Task<TrackDetailReturn> GetTrack(int id);
        Task<IEnumerable<TrackReturn>> GetMyCollection(string OwnerOfPublicAddress);
        Task<Track> GetTrackForMinting(int id);
        Task<int> PostTrack(TrackModel track);
        Task<bool> DeleteTrack(int id);
        Task<bool> UpdateTrack(int trackId, string title, string description, string isMinted, string isAuctioned, string isListed, string isSold);
        Task<bool> SetIsListed(int trackId, bool isListed);
        Task<bool> SetIsSold(SetAsSoldModel model);
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
            return await _context.Tracks.Where(w => w.IsSold == false && w.IsMinted==true).Include(t => t.Auction)
                .Include(t=>t.OwnerOf)
                .Include(t=>t.Listing)
                .Select(x => new TrackReturn
            {
                AuctionEnding = x.Auction != null ? (x.Auction.Ending- DateTime.Now).TotalSeconds : null,
                IsAuctioned = x.IsAuctioned,
                IsListed = x.IsListed,
                Id = x.Id,
                FileLocation = x.FileLocation.Split(new[] { "wwwroot" }, StringSplitOptions.None)[1],
                ImageLocation = x.ImageLocation.Split(new[] { "wwwroot" }, StringSplitOptions.None)[1],
                OwnerOfProfilePic = x.OwnerOf.ProfilePic,
                OwnerOfName = x.OwnerOf.Name,
                Title = x.Title,
                Price = x.Auction != null ? x.Auction.Price : x.Listing.Price,
                AuctionContractAddress = x.Auction != null ? x.Auction.ContractAddress : null
                }).ToListAsync();
        }

        public async Task<TrackDetailReturn> GetTrack(int id)
        {
            var track = await _context.Tracks.Include(x => x.OwnerOf).Include(x=>x.Auction).ThenInclude(a=>a.Bids).Include(x=>x.Listing)
                .FirstOrDefaultAsync(t => t.Id == id);
            TrackDetailReturn trackReturn = new()
            {
                Auction = track.Auction,
                AuctionId = track.AuctionId,
                FileLocation = track.FileLocation.Split(new[] { "wwwroot" }, StringSplitOptions.None)[1],
                ImageLocation = track.ImageLocation.Split(new[] { "wwwroot" }, StringSplitOptions.None)[1],
                Title = track.Title,
                Description = track.Description,
                Id = track.Id,
                IsAuctioned = track.IsAuctioned,
                IsListed = track.IsListed,
                IsMinted = track.IsMinted,
                IsSold = track.IsSold,
                ListingId = track.ListingId,
                OwnerOf = track.OwnerOf,
                OwnerOfPublicAddress = track.OwnerOfPublicAddress,
                Timestamp = track.Timestamp,
                TokenId = track.TokenId,
                AuctionEnding = track.Auction != null ? (track.Auction.Ending - DateTime.Now).TotalSeconds : null,
                Listing = track.Listing
            };
            return trackReturn;
        }

        public async Task<IEnumerable<TrackReturn>> GetMyCollection(string OwnerOfPublicAddress)
        {
            return await _context.Tracks.Where(w => w.IsSold == false && w.IsMinted == true && w.OwnerOfPublicAddress == OwnerOfPublicAddress)
               .Include(t => t.Auction)
               .Include(t => t.OwnerOf)
               .Include(t => t.Listing)
               .Select(x => new TrackReturn
               {
                   AuctionEnding = x.Auction != null ? (x.Auction.Ending - DateTime.Now).TotalSeconds : null,
                   IsAuctioned = x.IsAuctioned,
                   IsListed = x.IsListed,
                   Id = x.Id,
                   FileLocation = x.FileLocation.Split(new[] { "wwwroot" }, StringSplitOptions.None)[1],
                   ImageLocation = x.ImageLocation.Split(new[] { "wwwroot" }, StringSplitOptions.None)[1],
                   OwnerOfProfilePic = x.OwnerOf.ProfilePic,
                   OwnerOfName = x.OwnerOf.Name,
                   Title = x.Title,
                   Price = x.Auction != null ? x.Auction.Price : x.Listing.Price,
                   AuctionContractAddress = x.Auction != null ? x.Auction.ContractAddress : null
               }).ToListAsync();
        }
        public async Task<Track> GetTrackForMinting(int id)
        {
            var track = await _context.Tracks.Include(x => x.OwnerOf).Include(x => x.Auction).FirstOrDefaultAsync(t => t.Id == id);
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

        public async Task<bool> SetIsSold(SetAsSoldModel model)
        {
            Track track = new() { Id = model.TrackId, IsSold = true, OwnerOfPublicAddress = model.To };
                                  //Listing = null, ListingId = null, Auction= null, AuctionId= null};
            _context.Entry(track).Property(x => x.IsSold).IsModified = true;
            _context.Entry(track).Property(x => x.OwnerOfPublicAddress).IsModified = true;
            // TODO: clear listings and auctions tied to the track
            //_context.Entry(track).Reference(x => x.Listing).IsModified = true;
            //_context.Entry(track).Reference(x => x.ListingId).IsModified = true;
            //_context.Entry(track).Reference(x => x.Auction).IsModified = true;
            //_context.Entry(track).Reference(x => x.AuctionId).IsModified = true;

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
