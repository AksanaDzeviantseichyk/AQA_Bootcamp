using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Task_9.Core.Contracts;
using Task_9.Core.Models.Requests;
using Task_9.Core.Models.Responses.Base;
using Task_9.Core.Utils;

namespace Task_9.Core.Providers
{
    public class UserServiceProvider: IUserServiceProvider
    {
        private readonly IUserServiceClient _userServiceClient;
        private readonly UserGenerator _userGenerator;

        public UserServiceProvider(IUserServiceClient client,
            UserGenerator generator)
        {
            _userServiceClient = client;
            _userGenerator = generator;
        }

        public async Task<CommonResponse<int>> RegisterValidUser()
        {
            var request = _userGenerator.GenerateRegisterNewUserRequest();
            return await _userServiceClient.RegisterNewUser(request);
        }
        public async Task<CommonResponse<int>> RegisterValidUser(RegisterNewUserRequest request)
        {
            return await _userServiceClient.RegisterNewUser(request);
        }
        public async Task<CommonResponse<int>> RegisterValidUser(string condition)
        {
            switch (condition.Trim())
            {
                case "emptyFields":
                    return await RegisterValidUser(_userGenerator.GenerateEmptyUserFieldsRequest());
                case "nullFields":
                    return await RegisterValidUser(_userGenerator.GenerateNullUserFieldsRequest());
                case "length1SymbolFields":
                    return await RegisterValidUser(_userGenerator.GenerateUserFieldsWithLength1SymbolRequest());
                case "length100MoreSymbolsFields":
                    return await RegisterValidUser(_userGenerator.GenerateUserFieldsWithLength100MoreSymbolRequest());
                case "upperCaseFields":
                    return await RegisterValidUser(_userGenerator.GenerateUpperCaseUserFieldsRequest());
                case "digitFields":
                    return await RegisterValidUser(_userGenerator.GenerateUserFieldsWithDigitsRequest());
                case "specialCharactersFields":
                    return await RegisterValidUser(_userGenerator.GenerateUserFieldsWithSpecialCharactersRequest());
                 
                default: return null;
            }
            
        }
        public async Task<int> GetNotExistUserId()
        {
            var request = await RegisterValidUser();
            return request.Body+10;
        }
        public async Task<int> GetActiveUserId()
        {
            var request = await RegisterValidUser();
            await SetTrueStatusExistUser(request.Body);
            return request.Body;
        }

        public async Task<int> GetNotActiveUserId()
        {
            var request = await RegisterValidUser();
            return request.Body;
        }
        public async Task<CommonResponse<object>> DeleteExistUser(int userId)
        {
            return await _userServiceClient.DeleteUser(userId);
        }
        public async Task<CommonResponse<object>> DeleteNotExistUser(int notExistUserId)
        {
            return await _userServiceClient.DeleteUser(notExistUserId);
        }
        public async Task<CommonResponse<object>> SetTrueStatusExistUser(int userId)
        {
            return await _userServiceClient.SetUserStatus(userId, true);
        }
        public async Task<CommonResponse<object>> SetFalseStatusExistUser(int userId)
        {
            return await _userServiceClient.SetUserStatus(userId, false);
        }
        public async Task<CommonResponse<object>> SetTrueStatusNotExistUser(int notExistUserId)
        {
            return await _userServiceClient.SetUserStatus(notExistUserId, true);
        }
        public async Task<CommonResponse<object>> SetFalseStatusNotExistUser(int notExistUserId)
        {
            return await _userServiceClient.SetUserStatus(notExistUserId, false);
        }
        public async Task<CommonResponse<object>> SetUserStatus(int UserId, bool status)
        {
            return await _userServiceClient.SetUserStatus(UserId, status);
        }
        public async Task<CommonResponse<bool>> GetStatusExistUser(int userId)
        {
            return await _userServiceClient.GetUserStatus(userId);
        }
        public async Task<CommonResponse<bool>> GetStatusNotExistUser(int notExistUserId)
        {
            return await _userServiceClient.GetUserStatus(notExistUserId);
        }

    }
}
