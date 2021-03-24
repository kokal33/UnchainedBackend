using Microsoft.Extensions.Configuration;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;

namespace UnchainedBackend.Helpers
{
    public interface IEthHelper {
        Web3 GetWeb3(string accountAddress);
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
    }
}
