using Microsoft.AspNetCore.Http;


namespace UnchainedBackend.Models.PartialModels
{
    public class TrackModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
        public IFormFile CoverImage { get; set; }
        public string OwnerOfPublicAddress { get; set; }
        public int Price { get; set; }
    }
}
