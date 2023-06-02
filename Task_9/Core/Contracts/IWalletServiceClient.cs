using Task_9.Core.Models.Requests;
using Task_9.Core.Models.Responses;
using Task_9.Core.Models.Responses.Base;

namespace Task_9.Core.Contracts
{
    public interface IWalletServiceClient
    {
        Task<CommonResponse<decimal>> GetBalance(int userId);
        Task<CommonResponse<Guid>> BalanceCharge(BalanceChargeRequest request);
        Task<CommonResponse<Guid>> RevertTransaction(Guid transactionId);
        Task<CommonResponse<List<GetTransactionInfoResponse>>> GetTransaction(int userId);
        IDisposable Subscribe(IObserver<int> observer);
    }
}
