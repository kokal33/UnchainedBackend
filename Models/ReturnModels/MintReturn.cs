namespace UnchainedBackend.Models.ReturnModels
{
    public class MintReturn
    {
        public string TransactionHash { get; set; }
        public MetadataModel Metadata { get; set; }
        public int TokenId { get; set; }
    }
}
