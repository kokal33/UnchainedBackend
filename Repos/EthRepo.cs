using Microsoft.Extensions.Configuration;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Threading.Tasks;
using UnchainedBackend.Helpers;
using UnchainedBackend.Models.PartialModels;
using UnchainedBackend.UnchainedToken;
using UnchainedBackend.UnchainedToken.ContractDefinition;

namespace UnchainedBackend.Repos
{

    public interface IEthRepo
    {
        Task<string> DeployAndCall(DeployContractModel model);
        Task<int> GetBalanceOfAddress(GetAccountBalanceModel model);
        Task<TransactionReceipt> MintWithTokenURI(MintWithTokenURIModel model);
        Task<int> GetContractSupply();
        Task<string> GetTokenURI(GetTokenURIModel model);
        Task<string> TransferTo(TransferToModel model);
        Task<string> OwnerOf(OwnerOfModel model);
        string VerifySignature(SignatureModel model);
     }

    public class EthRepo : IEthRepo
    {
        private readonly IEthHelper _ethHelper;
        private readonly IConfiguration _configuration;

        public EthRepo(IEthHelper ethHelper, IConfiguration configuration) {
            _ethHelper = ethHelper;
            _configuration = configuration;
        }

        public async Task<int> GetBalanceOfAddress(GetAccountBalanceModel model) {
            var contractAddress = _configuration["Ethereum:ContractAddress"];
            var web3 = _ethHelper.GetWeb3(null);

            var balanceOfFunctionMessage = new BalanceOfFunction()
            {
                Owner = model.Address
            };

            var balanceHandler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();
            var balance = await balanceHandler.QueryAsync<int>(contractAddress, balanceOfFunctionMessage);
            return balance;
        }

        public async Task<string> DeployAndCall(DeployContractModel model)
        {
            var web3 = _ethHelper.GetWeb3(null);

            var deployment = new UnchainedTokenDeployment
            {
                Name = model.Name,
                Symbol = model.Symbol
            };
            var service = await UnchainedTokenService.DeployContractAndGetServiceAsync(web3, deployment);
            return service.ContractHandler.ContractAddress;
        }

        public async Task<TransactionReceipt> MintWithTokenURI(MintWithTokenURIModel model) {
            var contractAddress = _configuration["Ethereum:ContractAddress"];
            var web3 = _ethHelper.GetWeb3(null);

            var tokenURIFunction = new MintWithTokenURIFunction()
            {
                To = model.To,
                TokenURI = model.Metadata
            };

            var mintHandler = web3.Eth.GetContractTransactionHandler<MintWithTokenURIFunction>();
            var tokenURI = await mintHandler.SendRequestAndWaitForReceiptAsync(contractAddress, tokenURIFunction);
            return tokenURI;
        }

        public async Task<int> GetContractSupply()
        {
            var privateAddress = _configuration["Ethereum:PrivateAddress"];
            var contractAddress = _configuration["Ethereum:ContractAddress"];
            var web3 = _ethHelper.GetWeb3(privateAddress);

            var totalSupplyFunction = new TotalSupplyFunction();
            var mintHandler = web3.Eth.GetContractQueryHandler<TotalSupplyFunction>();
            var totalSupply = await mintHandler.QueryAsync<int>(contractAddress, totalSupplyFunction);

            return totalSupply;
        }

        public async Task<string> GetTokenURI(GetTokenURIModel model)
        {
            var privateAddress = _configuration["Ethereum:PrivateAddress"];
            var contractAddress = _configuration["Ethereum:ContractAddress"];

            var web3 = _ethHelper.GetWeb3(privateAddress);
            var tokenURIFunction = new TokenURIFunction() {
                TokenId = model.TokenId
            };

            var mintHandler = web3.Eth.GetContractQueryHandler<TokenURIFunction>();
            var totalSupply = await mintHandler.QueryAsync<string>(contractAddress, tokenURIFunction);
            return totalSupply;
        }

        public async Task<string> TransferTo(TransferToModel model)
        {
            var privateAddress = _configuration["Ethereum:PrivateAddress"];
            var contractAddress = _configuration["Ethereum:ContractAddress"];

            var web3 = _ethHelper.GetWeb3(null);
            var transferFunction = new TransferFromFunction()
            {
                From = privateAddress,
                To = model.To,
                TokenId  =model.TokenId
            };

            var mintHandler = web3.Eth.GetContractTransactionHandler<TransferFromFunction>();
            var result = await mintHandler.SendRequestAndWaitForReceiptAsync(contractAddress, transferFunction);
            return result.TransactionHash;
        }

        public async Task<string> OwnerOf(OwnerOfModel model)
        {
            var contractAddress = _configuration["Ethereum:ContractAddress"];

            var web3 = _ethHelper.GetWeb3(null);
            var ownerOfFunction = new OwnerOfFunction()
            {
                TokenId = model.TokenId
            };

            var mintHandler = web3.Eth.GetContractQueryHandler<OwnerOfFunction>();
            var result = await mintHandler.QueryAsync<string>(contractAddress, ownerOfFunction);
            return result;
        }

        public string VerifySignature(SignatureModel model)
        {
            string msg = "Hello, Guest! Please sign this message in order to login";
            string signature = "0xc01fb8beb629efc5814c1dedb4c90343f9af32cbdf9d2766c595e6b6c5e3e9ea6b890cbc267c939f11e73057136c100d1e88a3793710317a5b645bf2cd1bae5b1b";
            var signer = new EthereumMessageSigner();
            return signer.EncodeUTF8AndEcRecover(msg, signature);
        }
    }
}
