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
    public  interface IWalletServiceProvider
    {
        Task<CommonResponse<decimal>> GetBalance(int userId);
        Task<CommonResponse<Guid>> BalanceCharge(int userId, decimal amount);
        Task<CommonResponse<Guid>> BalanceCharge(int userId);
        Task<Guid> GetChargeTransactionId(int userId, decimal amount);
        Task<Guid> GetChargeTransactionId(int userId);
        Task<CommonResponse<Guid>> RevertExistTransaction(Guid transactionId);
        Task<CommonResponse<Guid>> RevertWrongTransaction();
        Task<CommonResponse<List<GetTransactionInfoResponse>>> GetTransaction(int userId);

    }
}
