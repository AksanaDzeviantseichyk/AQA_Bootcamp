using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_9.Core.Contracts;
using Task_9.Core.Extensions;
using Task_9.Core.Models.Requests;
using Task_9.Core.Models.Responses;
using Task_9.Core.Models.Responses.Base;
using Task_9.Tests;

namespace Task_9.Core.Clients
{
    public class WalletServiceClient: IWalletServiceClient, IObservable<Int32>
    {
        private static readonly Lazy<WalletServiceClient> _lazyClient = new Lazy<WalletServiceClient>(() => new WalletServiceClient());
        public static WalletServiceClient Instance => _lazyClient.Value;
        private readonly HttpClient _client = new HttpClient();
        private readonly string _baseUrl = "https://walletservice-uat.azurewebsites.net";
        private readonly ConcurrentBag<IObserver<Int32>> _chargeBalanceObservers = new ConcurrentBag<IObserver<Int32>>();


        public async Task<CommonResponse<decimal>> GetBalance(Int32 userId)
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

        public async Task<CommonResponse<List<GetTransactionInfoResponse>>> GetTransaction(Int32 userId)
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

        public void NotifyChargeBalanceObservers(Int32 userId)
        {

            foreach (IObserver<Int32> observer in _chargeBalanceObservers)
            {
                observer.OnNext(userId);
            }
        }
    }
}
