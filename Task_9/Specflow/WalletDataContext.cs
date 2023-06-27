using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public Guid TransactionId;
    }
}
