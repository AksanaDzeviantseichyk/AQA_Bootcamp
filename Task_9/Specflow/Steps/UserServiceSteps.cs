using Task_9.Core.Contracts;
using Task_9.Core.Observers;
using TechTalk.SpecFlow;

namespace Task_9.Specflow.Steps
{
    [Binding]
    public class UserServiceSteps
    {
        private readonly UserDataContext _userContext;
        private readonly IUserServiceProvider _userProvider;
        private readonly RegisterUserObserver _registerUserObserver;
        private readonly DeleteAndChargeObserver _deleteAndChargeObserver;

        public UserServiceSteps(UserDataContext userContext,
            IUserServiceProvider userProvider, 
            RegisterUserObserver registerUserObserver,
            DeleteAndChargeObserver deleteAndChargeObserver)
        {
            _userContext = userContext;
            _userProvider = userProvider;
            _registerUserObserver = registerUserObserver;
            _deleteAndChargeObserver = deleteAndChargeObserver;
        }

        [When(@"register valid user")]
        public async Task WhenRegisterValidUser()
        {
            _userContext.RegisterNewUserResponse = await _userProvider.RegisterValidUser();
        }

        [When(@"register valid user with (.*)")]
        public async Task WhenRegisterValidUserWith(string condition)
        {
            _userContext.RegisterNewUserResponse = await _userProvider.RegisterValidUser(condition);
        }

        [When(@"register (.*) valid user[s]?")]
        public async Task WhenRegisterValidUsers(int count)
        {
            _userContext.NewUserIds = Enumerable.Empty<int>();
            for (int i = 0; i < count; i++)
            {
                var response = await _userProvider.RegisterValidUser();
                _userContext.NewUserIds = _userContext.NewUserIds.Append(response.Body);
            }

        }

        [Given(@"get not active user id")]
        [When(@"get not active user id")]
        public async Task WhenGetNotActiveUserId()
        {
            _userContext.UserId = await _userProvider.GetNotActiveUserId();
        }

        [Given(@"get active user id")]
        public async Task GivenGetActiveUserId()
        {
            _userContext.UserId = await _userProvider.GetActiveUserId();
        }

        [When(@"delete user")]
        public async Task WhenDeleteUser()
        {
            _userContext.DeleteUserResponse = await _userProvider.DeleteExistUser(_userContext.UserId);
        }

        
         [Given(@"get not exist user id")]
         public async Task GivenGetNotExistUserId()
         {
            _userContext.UserId = await _userProvider.GetNotExistUserId();
         }

         [When(@"get user status")]
         public async Task WhenGetUserStatus()
         {
            _userContext.GetUserStatusResponse = await _userProvider.GetStatusNotExistUser(_userContext.UserId);
         }
                
        [When(@"set (.*) user status")]
        public async Task WhenSetIsActiveUserStatus(string isActive)
        {
            var isActives = isActive.Split("-").Select(bool.Parse).ToArray(); ;
            foreach(var status in isActives)
                _userContext.SetUserStatusResponse = await _userProvider.SetUserStatus(_userContext.UserId, status);
        }

    }
}
