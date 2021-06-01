using System;

namespace UnchainedBackend.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageLocation { get; set; }
        public string FileLocation { get; set; }
        public bool IsMinted { get; set; } = false;
        public bool IsAuctioned { get; set; } = false;
        public bool IsListed { get; set; } = false;
        public bool IsSold { get; set; } = false;
        public int TokenId { get; set; }
        public double Price { get; set; }
        public int TypeOfListing { get; set; }
        public virtual User OwnerOf { get; set; }
        public string OwnerOfPublicAddress { get; set; }
        public virtual Auction Auction { get; set; }
        public string AuctionId { get; set; }
        public virtual Listing Listing { get; set; }
        public string ListingId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
