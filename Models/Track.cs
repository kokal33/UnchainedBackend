﻿using System.Collections.Generic;

namespace UnchainedBackend.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public virtual User OwnerOf { get; set; }
        public string OwnerOfPublicAddress { get; set; }
        public bool isMinted { get; set; } = false;
        public bool isAuctioned { get; set; } = false;
        public bool isListed { get; set; } = false;
        public virtual IEnumerable<Bid> Bids { get; set; }
    }
}
