using System.Collections.Concurrent;
using Task_9.Core.Models.Responses;
using Task_9.Core.Models.Responses.Base;

namespace Task_9.Specflow
{
    public class WalletDataContext
    {
        public CommonResponse<decimal> GetBalanceResponse;
        public CommonResponse<Guid> BalanceChargeResponse;
        public CommonResponse<Guid> RevertTransactionResponse;
        public CommonResponse<List<GetTransactionInfoResponse>> GetTransactionResponse;
        public ConcurrentDictionary<decimal, Guid> BalanceChargeDictionary = new ConcurrentDictionary<decimal, Guid>();
        public ConcurrentDictionary<decimal, Guid> RevertTransactionDictionary = new ConcurrentDictionary<decimal, Guid>();
        public Guid TransactionId;
    }
}
