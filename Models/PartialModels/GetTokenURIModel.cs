using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnchainedBackend.Models.PartialModels
{
    public class GetTokenURIModel
    {
        public int TokenId { get; set; }
        public string FromAddress { get; set; }
    }
}
