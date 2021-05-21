using Microsoft.AspNetCore.Http;

namespace UnchainedBackend.Models.PartialModels
{
    public class MintModel
    {
        public string To { get; set; }
        public int TrackId { get; set; }
        public string Password { get; set; }
    }
}
