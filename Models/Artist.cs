using System.ComponentModel.DataAnnotations;

namespace UnchainedBackend.Models
{
    public class Artist : User
    {
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }
        public string SoundCloud { get; set; }
        public string BandCamp { get; set; }
        public string Beatport { get; set; }
    }
}
