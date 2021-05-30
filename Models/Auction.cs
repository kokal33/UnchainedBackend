using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnchainedBackend.Models
{
    public class Auction
    {
        [Key]
        public int Id { get; set; }
        public DateTime Started { get; set; }
        public DateTime Ending { get; set; }
        public bool IsEnded { get; set; }
        public IEnumerable<Bid> Bids { get; set; }
        public virtual Track Track { get; set; }
        public virtual int TrackId { get; set; }
    }
}
