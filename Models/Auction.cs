using System.ComponentModel.DataAnnotations;

namespace UnchainedBackend.Models
{
    public class Auction
    {
        [Key]
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }

    }
}
