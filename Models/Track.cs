namespace UnchainedBackend.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public virtual User OwnerOf { get; set; }
        public int OwnerOfId { get; set; }
    }
}
