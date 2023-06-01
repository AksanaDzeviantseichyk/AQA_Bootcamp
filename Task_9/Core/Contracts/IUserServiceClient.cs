using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_9.Core.Models.Requests;
using Task_9.Core.Models.Responses.Base;

namespace Task_9.Core.Contracts
{
    public interface IUserServiceClient
    {
        Task<CommonResponse<int>> RegisterNewUser(RegisterNewUserRequest request);
        Task<CommonResponse<object>> DeleteUser(int userId);
        Task<CommonResponse<object>> SetUserStatus(int userId, bool newStatus);
        Task<CommonResponse<bool>> GetUserStatus(int userId);
    }
}
