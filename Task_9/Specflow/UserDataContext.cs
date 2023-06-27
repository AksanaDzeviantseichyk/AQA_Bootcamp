
using System.Security.Policy;
using Task_9.Core.Models.Responses.Base;

namespace Task_9.Specflow
{
    public class UserDataContext
    {
        public CommonResponse<int> RegisterNewUserResponse;
        public CommonResponse<object> SetUserStatusResponse;
        public CommonResponse<bool> GetUserStatusResponse;
        public CommonResponse<object> DeleteUserResponse;
        public IEnumerable<int> NewUserIds;
        public int UserId;


    }
}
