﻿using Microsoft.Extensions.Configuration;
using Nethereum.RPC.Eth.DTOs;
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
        Task<TransactionReceipt> MintWithTokenURI(MintModel model);
        Task<int> GetContractSupply();
        Task<string> GetTokenURI(GetTokenURIModel model);
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
            var privateKey = "3f95ca38499439b8adee49d194522154c48bdfe1a2529c4865b38b54970aef74";
            var contractAddress = _configuration["Ethereum:ContractAddress"];
            var account = new Account(privateKey);
            var web3 = new Web3(account, "https://data-seed-prebsc-1-s3.binance.org:8545");

            var balanceOfFunctionMessage = new BalanceOfFunction()
            {
                Owner = account.Address
            };

            var balanceHandler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();
            var balance = await balanceHandler.QueryAsync<int>(contractAddress, balanceOfFunctionMessage);
            return balance;
        }

        public async Task<string> DeployAndCall(DeployContractModel model)
        {
            var privateKey = "3f95ca38499439b8adee49d194522154c48bdfe1a2529c4865b38b54970aef74";
            var account = new Account(privateKey);
            var web3 = new Web3(account, "https://data-seed-prebsc-1-s3.binance.org:8545");

            var deployment = new UnchainedTokenDeployment
            {
                Name = model.Name,
                Symbol = model.Symbol,
                BaseURI = model.BaseURI
            };
            var service = await UnchainedTokenService.DeployContractAndGetServiceAsync(web3, deployment);
            return service.ContractHandler.ContractAddress;
        }

        //TODO: Maybe dont send file to mint function, we don't need it
        public async Task<TransactionReceipt> MintWithTokenURI(MintModel model) {
            var privateKey = _configuration["Ethereum:PrivateKey"];
            var contractAddress = _configuration["Ethereum:ContractAddress"];

            var account = new Account(privateKey);
            var web3 = new Web3(account, "https://data-seed-prebsc-1-s3.binance.org:8545");
            
            var tokenURIFunction = new MintWithTokenURIFunction()
            {
                To = account.Address,
                TokenURI = model.TokenURI,
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

            var tokenURIFunction = new TotalSupplyFunction();
            var mintHandler = web3.Eth.GetContractQueryHandler<TotalSupplyFunction>();
            var totalSupply = await mintHandler.QueryAsync<int>(contractAddress, tokenURIFunction);

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
    }
}
