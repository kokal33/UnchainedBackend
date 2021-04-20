using Microsoft.Extensions.Configuration;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using UnchainedBackend.Models.PartialModels;

namespace UnchainedBackend.Helpers
{
    public interface IEthHelper {
        Web3 GetWeb3(string accountAddress);
        bool VerifySignature(SignatureModel model);
    }
    public class EthHelper: IEthHelper
    {
        private readonly IConfiguration _configuration;

        public EthHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Web3 GetWeb3(string accountAddress)
        {
            var privateKey = accountAddress ?? _configuration["Ethereum:PrivateKey"];
            var contractAddress = _configuration["Ethereum:ContractAddress"];

            var account = new Account(privateKey);
            return new Web3(account, "https://data-seed-prebsc-1-s3.binance.org:8545");
        }

        // METAMASK returns uppercase addresses, and BINANCE returns normal
        public bool VerifySignature(SignatureModel model) {
            string msg = "Hello, Guest! Please sign this message in order to login";
            var signer = new EthereumMessageSigner();
            var address = signer.EncodeUTF8AndEcRecover(msg, model.Signature);
            if (address.ToUpper() == model.PublicAddress.ToUpper())
                return true;
            return false;
        }
    }
}
