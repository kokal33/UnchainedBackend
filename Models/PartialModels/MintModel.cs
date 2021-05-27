using Microsoft.AspNetCore.Http;

namespace UnchainedBackend.Models.PartialModels
{
    public class MintModel
    {
        public int TrackId { get; set; }
        // TODO: Change encryption to signature of wallet
        public string Password { get; set; } = "Unchained";
    }
}
