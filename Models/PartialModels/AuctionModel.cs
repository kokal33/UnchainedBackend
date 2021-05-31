using System;

namespace UnchainedBackend.Models.PartialModels
{
    public class AuctionModel
    {
        public DateTime Started { get; set; }
        public DateTime Ending { get; set; }
        public int TrackId { get; set; }
        public double Price { get; set; }
    }
}
