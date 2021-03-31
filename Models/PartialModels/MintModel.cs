using Microsoft.AspNetCore.Http;

namespace UnchainedBackend.Models.PartialModels
{
    public class MintModel
    {
        public string To { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public IFormFile File { get; set; }
        public IFormFile CoverImage { get; set; }
        public string Password { get; set; }
    }
}
