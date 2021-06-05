
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnchainedBackend.Data;
using UnchainedBackend.Models;

namespace UnchainedBackend.Repos
{
    public interface IMarketplaceRepo
    {
        Task PostListing(Listing listing);
    }
    public class MarketplaceRepo : IMarketplaceRepo
    {
        private readonly ApplicationContext _context;

        public MarketplaceRepo(ApplicationContext context)
        {
            _context = context;
        }

        public async Task PostListing(Listing listing)
        {
            var track = await _context.Tracks.Include(x=>x.Listing).FirstOrDefaultAsync(x=>x.Id == listing.TrackId);
            listing.Created = DateTime.Now;
            track.Listing = listing;
            track.IsListed = true;
            _context.Entry(track).Property(x => x.IsListed).IsModified = true;
            _context.Entry(listing).State = EntityState.Added;
            _context.Tracks.Update(track);
            _context.SaveChanges();
        }
    }
}
