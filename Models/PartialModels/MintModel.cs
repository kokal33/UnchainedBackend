using Microsoft.AspNetCore.Http;

namespace UnchainedBackend.Models.PartialModels
{
    public class MintModel
    {
        public string To { get; set; }
        public string TokenURI { get; set; }
        public IFormFile File { get; set; }
    }
}
