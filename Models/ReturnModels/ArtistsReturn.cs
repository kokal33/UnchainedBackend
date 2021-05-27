using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnchainedBackend.Models.ReturnModels
{
    public class ArtistsReturn
    {
        public string PublicAddress { get; set; }
        public string Name { get; set; }
        public string ProfilePic { get; set; }
        public bool Verified { get; set; }
    }
}