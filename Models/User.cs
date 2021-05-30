using System.ComponentModel.DataAnnotations;

namespace UnchainedBackend.Models
{
    public class User
    {
        [Key]
        public string PublicAddress { get; set; }
        public string Name { get; set; }
        public string Signature { get; set; }
        public string Bio { get; set; }
        public string ProfilePic { get; set; }
        public bool IsArtist { get; set; } = false;
        public bool Verified { get; set; } = false;
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }
        public string SoundCloud { get; set; }
        public string BandCamp { get; set; }
        public string Beatport { get; set; }
    }
}
