
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
            track.Listing = listing;
            _context.Entry(track).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
