using Microsoft.Extensions.Configuration;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using UnchainedBackend.Models.PartialModels;

namespace UnchainedBackend.Helpers
{
    public interface IEthHelper {
        Web3 GetWeb3();
        bool VerifySignature(SignatureModel model);
    }
    public class EthHelper: IEthHelper
    {
        private readonly IConfiguration _configuration;

        public EthHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Web3 GetWeb3()
        {
            var privateKey = _configuration["Ethereum:PrivateKey"];
            var maticChain = _configuration["Ethereum:MaticChain"];

            var account = new Account(privateKey, 80001);
            return new Web3(account, maticChain);
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
