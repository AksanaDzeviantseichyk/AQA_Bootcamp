using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Text;
using Task_9.Core.Contracts;
using Task_9.Core.Enum;
using Task_9.Core.Extensions;
using Task_9.Core.Models.Requests;
using Task_9.Core.Models.Responses;
using Task_9.Core.Models.Responses.Base;
using Task_9.Core.Observers;

namespace Task_9.Core.Clients
{
    public class WalletServiceClient: IWalletServiceClient
    {
        private readonly HttpClient _client =new HttpClient();
        private readonly string _baseUrl = "https://walletservice-uat.azurewebsites.net";
        private readonly ConcurrentBag<IObserver<UserActionInfo>> _userObservers = new ConcurrentBag<IObserver<UserActionInfo>>();

        public async Task<CommonResponse<decimal>> GetBalance(int userId)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUrl}/Balance/GetBalance?userId={userId}"),
            };

            HttpResponseMessage response = await _client.SendAsync(httpRequestMessage);

            return await response.ToCommonResponse<decimal>();
        }
        public async Task<CommonResponse<Guid>> BalanceCharge(BalanceChargeRequest request)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUrl}/Balance/Charge"),
                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await _client.SendAsync(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                NotifyUserObservers(new UserActionInfo
                {
                    Id = request.UserId,
                    Action = UserAction.Charged
                });
            }

            return await response.ToCommonResponse<Guid>();
        }

        public async Task<CommonResponse<Guid>> RevertTransaction(Guid transactionId)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_baseUrl}/Balance/RevertTransaction?transactionId={transactionId}"),
            };

            HttpResponseMessage response = await _client.SendAsync(httpRequestMessage);

            return await response.ToCommonResponse<Guid>();
        }

        public async Task<CommonResponse<List<GetTransactionInfoResponse>>> GetTransaction(int userId)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUrl}/Balance/GetTransactions?userId={userId}"),
            };

            HttpResponseMessage response = await _client.SendAsync(httpRequestMessage);

            return await response.ToCommonResponse<List<GetTransactionInfoResponse>>();
        }

        public IDisposable Subscribe(IObserver<UserActionInfo> observer)
        {
            _userObservers.Add(observer);
            return null;
        }
        public void NotifyUserObservers(UserActionInfo info)
        {
            foreach (var observer in _userObservers)
            {
                observer.OnNext(info);
            }

        }

    }
}
