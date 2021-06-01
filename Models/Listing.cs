using System;
using System.ComponentModel.DataAnnotations;

namespace UnchainedBackend.Models
{
    public class Listing
    {
        [Key]
        public int Id { get; set; }
        public double Price { get; set; }
        public virtual Track Track { get; set; }
        public virtual int TrackId { get; set; }
        public DateTime Created { get; set; }

    }
}
