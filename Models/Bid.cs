using System;
using System.ComponentModel.DataAnnotations;

namespace UnchainedBackend.Models
{
    public class Bid
    {
        [Key]
        public string Id { get; set; }
        public string PublicAddress { get; set; }
        public string Signature { get; set; }
        public int AmountInBsc { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual Track Track { get; set; }
        public int TrackId { get; set; }
    }
}
