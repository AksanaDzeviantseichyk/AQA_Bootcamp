using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_9.Core.Models.Requests;
using Task_9.Core.Models.Responses;
using Task_9.Core.Models.Responses.Base;

namespace Task_9.Core.Contracts
{
    public interface IWalletServiceClient
    {
        Task<CommonResponse<decimal>> GetBalance(Int32 userId);
        Task<CommonResponse<Guid>> BalanceCharge(BalanceChargeRequest request);
        Task<CommonResponse<Guid>> RevertTransaction(Guid transactionId);
        Task<CommonResponse<List<GetTransactionInfoResponse>>> GetTransaction(Int32 userId);
    }
}
