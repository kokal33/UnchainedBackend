namespace UnchainedBackend.Models
{
    public class PendingArtist
    {
        public int Id { get; set; }
        public virtual User Artist { get; set; }
        public string ArtistPublicAddress { get; set; }

    }
}
