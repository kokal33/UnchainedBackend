using System;
using System.Collections.Generic;

namespace UnchainedBackend.Models.ReturnModels
{
    public class TrackReturn
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageLocation { get; set; }
        public string FileLocation { get; set; }
        public bool IsAuctioned { get; set; } = false;
        public bool IsListed { get; set; } = false;
        public string OwnerOfProfilePic { get; set; }
        public string OwnerOfName { get; set; }
        public double? AuctionEnding { get; set; }
        public double? Price { get; set; }
        public string AuctionContractAddress { get; set; }
    }
}
