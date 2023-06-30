using Task_9.Core.Models.Requests;
using Task_9.Core.Models.Responses.Base;
using Task_9.Core.Observers;

namespace Task_9.Core.Contracts
{
    public interface IUserServiceClient: IObservable<UserActionInfo>
    {
        Task<CommonResponse<int>> RegisterNewUser(RegisterNewUserRequest request);
        Task<CommonResponse<object>> DeleteUser(int userId);
        Task<CommonResponse<object>> SetUserStatus(int userId, bool newStatus);
        Task<CommonResponse<bool>> GetUserStatus(int userId);
        
    }
}
