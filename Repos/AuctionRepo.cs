
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnchainedBackend.Data;
using UnchainedBackend.Models;

namespace UnchainedBackend.Repos
{
    public interface IAuctionRepo
    {
        Task<IEnumerable<Auction>> GetActiveAuctions();
        Task<bool> SetAuction(Auction auction);
    }
    public class AuctionRepo : IAuctionRepo
    {
        private readonly ApplicationContext _context;

        public AuctionRepo(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Auction>> GetActiveAuctions()
        {
            return await _context.Auctions.Where(x => x.IsEnded == false).ToListAsync();
        }

        public async Task<bool> SetAuction(Auction auction) 
        {
            var track = await _context.Tracks.Include(x => x.Auction).FirstOrDefaultAsync(x => x.Id == auction.TrackId);
            track.Auction = auction;
            track.IsAuctioned = true;
            _context.Entry(track).Property(x => x.IsAuctioned).IsModified = true;
            _context.Entry(auction).State = EntityState.Added;
            _context.Tracks.Update(track);
            _context.SaveChanges();
            return true;
        }
    }
}
