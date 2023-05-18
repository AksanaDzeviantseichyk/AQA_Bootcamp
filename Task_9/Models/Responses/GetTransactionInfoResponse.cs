using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_9.Enum;

namespace Task_9.Models.Responses
{
    public class GetTransactionInfoResponse
    {
        public Int32 UserId { get; set; }
        public decimal Amount { get; set; }
        public Guid TransactionId { get; set; }
        public string Time { get; set; }
        public TransactionStatus Status { get; set; }
        public Guid? BaseTransactionId { get; set; }

    }
}
