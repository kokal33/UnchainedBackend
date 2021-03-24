using Nethereum.ABI.FunctionEncoding.Attributes;

namespace UnchainedBackend.Models.PartialModels
{
    public class DeployContractModel
    {
        [Parameter("string", "name", 1)]
        public string Name { get; set; }
        [Parameter("string", "symbol", 2)]
        public string Symbol { get; set; }
        [Parameter("string", "baseURI", 3)]
        public string BaseURI { get; set; }
    }
}
