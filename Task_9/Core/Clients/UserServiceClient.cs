using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Text;
using Task_9.Core.Contracts;
using Task_9.Core.Enum;
using Task_9.Core.Extensions;
using Task_9.Core.Models.Requests;
using Task_9.Core.Models.Responses.Base;
using Task_9.Core.Observers;

namespace Task_9.Core.Clients
{
    public class UserServiceClient: IUserServiceClient
    {
        private readonly HttpClient _client =new HttpClient();
        private readonly string _baseUrl = "https://userservice-uat.azurewebsites.net";
        private readonly ConcurrentBag<IObserver<UserActionInfo>> _userObservers = new ConcurrentBag<IObserver<UserActionInfo>>();

        public async Task<CommonResponse<int>> RegisterNewUser(RegisterNewUserRequest request)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUrl}/Register/RegisterNewUser"),
                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await _client.SendAsync(httpRequestMessage);
            var commonResponse = await response.ToCommonResponse<int>(); 
            if (response.IsSuccessStatusCode)
            {
                NotifyUserObservers(new UserActionInfo
                {
                    Id = commonResponse.Body,
                    Action = UserAction.Created
                });
            }

            return commonResponse;
        }

        public async Task<CommonResponse<object>> DeleteUser(int userId)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_baseUrl}/Register/DeleteUser?userId={userId}"),
            };

            HttpResponseMessage response = await _client.SendAsync(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                NotifyUserObservers(new UserActionInfo
                { 
                    Id = userId,
                    Action = UserAction.Deleted
                });
            }

            return await response.ToCommonResponse<object>();
        }

        public async Task<CommonResponse<object>> SetUserStatus(int userId, bool newStatus)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_baseUrl}/UserManagement/SetUserStatus?userId={userId}&newStatus={newStatus}"),
            };

            HttpResponseMessage response = await _client.SendAsync(httpRequestMessage);

            return await response.ToCommonResponse<object>();
        }

        public async Task<CommonResponse<bool>> GetUserStatus(int userId)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUrl}/UserManagement/GetUserStatus?userId={userId}"),
            };

            HttpResponseMessage response = await _client.SendAsync(httpRequestMessage);

            return await response.ToCommonResponse<bool>();
        }
        public void NotifyUserObservers (UserActionInfo info)
        {
            foreach (var observer in _userObservers)
            {
                observer.OnNext(info);
            }            
        }
        public IDisposable Subscribe(IObserver<UserActionInfo> observer)
        {
            _userObservers.Add(observer);
            return null;
        }
    }
}
