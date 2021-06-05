using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnchainedBackend.Data;
using UnchainedBackend.Models;
using UnchainedBackend.Models.PartialModels;

namespace UnchainedBackend.Repos
{
    public interface IBidsRepo
    {
        Task<IEnumerable<Bid>> GetBids();
        Task<Bid> GetBid(string publicAddress);
        Task<bool> PostBid(BidModel bid);
        Task<bool> DeleteBid(string publicAddress);
    }
    public class BidsRepo : IBidsRepo
    {
        private readonly ApplicationContext _context;

        public BidsRepo(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bid>> GetBids()
        {
            return await _context.Bids.ToListAsync();
        }

        public async Task<Bid> GetBid(string publicAddress)
        {
            var bid = await _context.Bids.FindAsync(publicAddress);
            return bid;
        }

        public async Task<bool> PostBid(BidModel bid)
        {
            Bid bidToSave = new() {
                Amount = bid.Amount,
                OwnerOfPublicAddress = bid.BidderAddress,
                Timestamp = DateTime.Now
            };
            var auction = await _context.Auctions.Include(x=>x.Bids).FirstOrDefaultAsync(x=>x.Id == bid.AuctionId);
            auction.Bids.Add(bidToSave);
            _context.Entry(bidToSave).State = EntityState.Added;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteBid(string publicAddress)
        {
            var bid = await _context.Bids.FindAsync(publicAddress);

            _context.Bids.Remove(bid);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
