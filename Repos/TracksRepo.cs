using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using UnchainedBackend.Data;
using UnchainedBackend.Helpers;
using UnchainedBackend.Models;
using UnchainedBackend.Models.PartialModels;

namespace UnchainedBackend.Repos
{
    public interface ITracksRepo
    {
        Task<IEnumerable<Track>> GetTracks();
        Task<Track> GetTrack(int id);
        Task<Track> PostTrack(TrackModel track);
        Task<bool> DeleteTrack(int id);
        Task<bool> UpdateTrack(int trackId, string title, string description, string isMinted, string isAuctioned, string isListed, string isSold);
        Task<bool> SetIsListed(int trackId, bool isListed);
        Task<bool> SetIsSold(int trackId, bool isSold);
        Task<bool> SetIsMinted(Track track, bool isMinted);
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

        public async Task<IEnumerable<Track>> GetTracks()
        {
            return await _context.Tracks.ToListAsync();
        }

        public async Task<Track> GetTrack(int id)
        {
            var track = await _context.Tracks.Include(x => x.OwnerOf).FirstOrDefaultAsync(t => t.Id == id);
            return track;
        }       

        public async Task<Track> PostTrack(TrackModel model)
        {
            // Write the track to storage
            string fileContent = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", model.OwnerOfPublicAddress, "Tracks");
            string filePath = Path.Combine(fileContent, model.File.FileName);
            (new FileInfo(filePath)).Directory.Create();
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                await model.File.CopyToAsync(fileStream);
            // Write the track image to store
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

            return newTrack;
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
                    track.isMinted = true;
                if (isMinted == "false")
                    track.isMinted = false;
            }
            if (isAuctioned != null)
            {
                if (isAuctioned == "true")
                    track.isAuctioned = true;
                if (isAuctioned == "false")
                    track.isAuctioned = false;
            }
            if (isListed != null)
            {
                if (isListed == "true")
                    track.isListed = true;
                if (isListed == "false")
                    track.isListed = false;
            }
            if (isSold != null)
            {
                if (isSold == "true")
                    track.isSold = true;
                if (isSold == "false")
                    track.isSold = false;
            }
            _context.Entry(track).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetIsListed(int trackId, bool isListed) 
        {
            Track track = new Track { Id = trackId, isListed = isListed };
            _context.Entry(track).Property(x => x.isListed).IsModified = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetIsMinted(Track track, bool isMinted)
        {
            _context.Entry(track).Property(x => x.isMinted).IsModified = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetIsSold(int trackId, bool isSold)
        {
            Track track = new Track { Id = trackId, isSold = isSold };
            _context.Entry(track).Property(x => x.isSold).IsModified = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetIsAuctioned(int trackId, bool isAuctioned)
        {
            Track track = new Track { Id = trackId, isAuctioned = isAuctioned };
            _context.Entry(track).Property(x => x.isAuctioned).IsModified = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
