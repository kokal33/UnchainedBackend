using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnchainedBackend.Models
{
    public class Bid
    {
        [Key]
        public int Id { get; set; }
        public virtual User OwnerOf { get; set; }
        public string OwnerOfPublicAddress { get; set; }
        public double Amount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
