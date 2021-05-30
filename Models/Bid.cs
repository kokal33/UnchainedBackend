using System;
using System.ComponentModel.DataAnnotations;

namespace UnchainedBackend.Models
{
    public class Bid
    {
        [Key]
        public string Id { get; set; }
        public virtual User OwnerOf { get; set; }
        public string OwnerOfPublicAddress { get; set; }
        public int Amount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
