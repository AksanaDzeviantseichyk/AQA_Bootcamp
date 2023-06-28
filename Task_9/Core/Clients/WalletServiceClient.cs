using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Text;
using Task_9.Core.Contracts;
using Task_9.Core.Extensions;
using Task_9.Core.Models.Requests;
using Task_9.Core.Models.Responses;
using Task_9.Core.Models.Responses.Base;

namespace Task_9.Core.Clients
{
    public class WalletServiceClient: IWalletServiceClient, IObservable<int>
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "https://walletservice-uat.azurewebsites.net";
        private readonly ConcurrentBag<IObserver<int>> _chargeBalanceObservers;
        public WalletServiceClient(HttpClient client, ConcurrentBag<IObserver<int>> chargeBalanceObservers)
        {
            _client = client;
            _chargeBalanceObservers = chargeBalanceObservers;
        }

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
                NotifyChargeBalanceObservers(request.UserId);
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

        public IDisposable Subscribe(IObserver<int> observer)
        {
            _chargeBalanceObservers.Add(observer);
            return null;
        }

        public void NotifyChargeBalanceObservers(int userId)
        {

            foreach (IObserver<int> observer in _chargeBalanceObservers)
            {
                observer.OnNext(userId);
            }
        }
    }
}
