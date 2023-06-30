using NUnit.Framework;

namespace Task_9.Tests
{
    [SetUpFixture]
    public class SetUpFixture : BaseTest
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _userClient.Subscribe(_userActionObserver);
            
            _walletClient.Subscribe(_userActionObserver);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDowm()
        {
            var deleteUsers = _userActionObserver
                .GetAllUsersToDelete();
            var tasks = deleteUsers
                .Select(userId => _userClient.DeleteUser(userId));

            await Task.WhenAll(tasks);
        }
    }
}
