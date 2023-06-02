using Task_9.Core.Models.Requests;
using Task_9.Core.Models.Responses.Base;

namespace Task_9.Core.Contracts
{
    public interface IUserServiceProvider
    {
        Task<CommonResponse<int>> RegisterValidUser();
        Task<CommonResponse<int>> RegisterValidUser(RegisterNewUserRequest request);
        Task<int> GetNotExistUserId();
        Task<int> GetActiveUserId();
        Task<int> GetNotActiveUserId();
        Task<CommonResponse<object>> DeleteExistUser(int userId);
        Task<CommonResponse<object>> DeleteNotExistUser(int notExistUserId);
        Task<CommonResponse<object>> SetTrueStatusExistUser(int userId);
        Task<CommonResponse<object>> SetFalseStatusExistUser(int userId);
        Task<CommonResponse<object>> SetTrueStatusNotExistUser(int notExistUserId);
        Task<CommonResponse<object>> SetFalseStatusNotExistUser(int notExistUserId);
        Task<CommonResponse<bool>> GetStatusExistUser(int userId);
        Task<CommonResponse<bool>> GetStatusNotExistUser(int notExistUserId);
    }
}
