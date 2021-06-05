using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnchainedBackend.Models.ReturnModels
{
    public class TrackDetailReturn: Track
    {
        public double? AuctionEnding { get; set; }
    }
}
