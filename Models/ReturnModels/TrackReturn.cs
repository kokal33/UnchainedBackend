using System;
using System.Collections.Generic;

namespace UnchainedBackend.Models.ReturnModels
{
    public class TrackReturn
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageLocation { get; set; }
        public string FileLocation { get; set; }
        public bool IsAuctioned { get; set; } = false;
        public bool IsListed { get; set; } = false;
        public int TokenId { get; set; }
        public string OwnerOfProfilePic { get; set; }
        public string OwnerOfPublicAddress { get; set; }
        public DateTime? AuctionEnding { get; set; }
        public string AuctionId { get; set; }
        public double? Price { get; set; }
        public DateTime Ending { get; set; }
    }
}
