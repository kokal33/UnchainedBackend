using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnchainedBackend.Models.PartialModels
{
    public class BidModel
    {
        public int AuctionId { get; set; }
        public string BidderAddress { get; set; }
        public double Amount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
