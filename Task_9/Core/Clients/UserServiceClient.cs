using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_9.Core.Extensions;
using Task_9.Core.Models.Requests;
using Task_9.Core.Models.Responses.Base;
using Task_9.Tests;

namespace Task_9.Core.Clients
{
    public class UserServiceClient: IObservable<Int32>
    {
        private static readonly Lazy<UserServiceClient> _lazyClient = new Lazy<UserServiceClient>(() => new UserServiceClient());
        public static UserServiceClient Instance => _lazyClient.Value;
        private readonly HttpClient _client = new HttpClient();
        private readonly string _baseUrl = "https://userservice-uat.azurewebsites.net";
        private readonly ConcurrentBag<RegisterUserObserver> _registerUserObservers = new ConcurrentBag<RegisterUserObserver>();
        private readonly ConcurrentBag<DeleteAndChargeObserver> _deleteUserObservers = new ConcurrentBag<DeleteAndChargeObserver>();

        public async Task<CommonResponse<int>> RegisterNewUser(RegisterNewUserRequest request)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUrl}/Register/RegisterNewUser"),
                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await _client.SendAsync(httpRequestMessage);
            var commonResponse = await response.ToCommonResponse<Int32>(); 
            if (response.IsSuccessStatusCode)
            {
                NotifyRegisterUserObservers(commonResponse.Body);
            }

            return commonResponse;
        }

        public async Task<CommonResponse<object>> DeleteUser(Int32 userId)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_baseUrl}/Register/DeleteUser?userId={userId}"),
            };

            HttpResponseMessage response = await _client.SendAsync(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                NotifyDeleteUserObservers(userId);
            }

            return await response.ToCommonResponse<object>();
        }

        public async Task<CommonResponse<object>> SetUserStatus(Int32 userId, bool newStatus)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_baseUrl}/UserManagement/SetUserStatus?userId={userId}&newStatus={newStatus}"),
            };

            HttpResponseMessage response = await _client.SendAsync(httpRequestMessage);

            return await response.ToCommonResponse<object>();
        }

        public async Task<CommonResponse<bool>> GetUserStatus(Int32 userId)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUrl}/UserManagement/GetUserStatus?userId={userId}"),
            };

            HttpResponseMessage response = await _client.SendAsync(httpRequestMessage);

            return await response.ToCommonResponse<bool>();
        }

        public IDisposable Subscribe(IObserver<Int32> observer)
        {
            if (observer is RegisterUserObserver)
            {
                _registerUserObservers.Add((RegisterUserObserver)(object)observer);
            }
            else if (observer is DeleteAndChargeObserver)
            {
                _deleteUserObservers.Add((DeleteAndChargeObserver)(object)observer);
            }
            return null;
        }
        

        public void NotifyRegisterUserObservers (Int32 userId)
        {
            foreach(RegisterUserObserver observer in _registerUserObservers)
            {
                observer.OnNext(userId);
            }
        }
        public void NotifyDeleteUserObservers(Int32 userId)
        {
            
            foreach (DeleteAndChargeObserver observer in _deleteUserObservers)
            {
                observer.OnNext(userId);
            }
        }
    }
}
