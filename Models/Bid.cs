using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UnchainedBackend.Models
{
    public class Bid
    {
        [Key]
        public string PublicAddress { get; set; }
        public string Signature { get; set; }
        public int AmountInBsc { get; set; }
        public string NFT { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
