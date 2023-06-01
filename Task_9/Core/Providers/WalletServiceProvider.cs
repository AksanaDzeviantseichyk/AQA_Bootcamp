using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Task_9.Core.Clients;
using Task_9.Core.Contracts;
using Task_9.Core.Models.Requests;
using Task_9.Core.Models.Responses;
using Task_9.Core.Models.Responses.Base;
using Task_9.Core.Utils;

namespace Task_9.Core.Providers
{
    public class WalletServiceProvider : IWalletServiceProvider
    {
        private readonly IWalletServiceClient _walletServiceClient;
        private readonly BalanceChargeGenerator _balanceChargeGenerator;

        public WalletServiceProvider(IWalletServiceClient client,
           BalanceChargeGenerator generator)
        {
            _walletServiceClient = client;
            _balanceChargeGenerator = generator;
        }
        public async Task<CommonResponse<Guid>> BalanceCharge(int userId, decimal amount)
        {
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(userId, amount);
            return  await _walletServiceClient.BalanceCharge(balanceChargeRequest);
        }

        public async Task<CommonResponse<Guid>> BalanceCharge(int userId)
        {
            var balanceChargeRequest = _balanceChargeGenerator.GenerateBalanceCharge(userId);
            return await _walletServiceClient.BalanceCharge(balanceChargeRequest);
        }

        public async Task<Guid> GetChargeTransactionId(int userId, decimal amount)
        {
            var response = await BalanceCharge(userId, amount);
            return response.Body;
        }
        public async Task<Guid> GetChargeTransactionId(int userId)
        {
            var response = await BalanceCharge(userId);
            return response.Body;
        }

        public async Task<CommonResponse<decimal>> GetBalance(int userId)
        {
            return await _walletServiceClient.GetBalance(userId);
        }

        public async Task<CommonResponse<List<GetTransactionInfoResponse>>> GetTransaction(int userId)
        {
            return await _walletServiceClient.GetTransaction(userId);
        }

        public async Task<CommonResponse<Guid>> RevertExistTransaction(Guid transactionId)
        {
            return await _walletServiceClient.RevertTransaction(transactionId);
        }

        public async Task<CommonResponse<Guid>> RevertWrongTransaction()
        {
            return await _walletServiceClient.RevertTransaction(new Guid());
        }
    }
}
