using Task_9.Core.Enum;

namespace Task_9.Core.Models.Responses
{
    public class GetTransactionInfoResponse
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public Guid TransactionId { get; set; }
        public DateTime Time { get; set; }
        public TransactionStatus Status { get; set; }
        public Guid? BaseTransactionId { get; set; }

    }
}
