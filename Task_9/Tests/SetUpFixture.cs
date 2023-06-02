using NUnit.Framework;

namespace Task_9.Tests
{
    [SetUpFixture]
    public class SetUpFixture: BaseTest
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _userClient.Subscribe(_registerUserObserver);
            _userClient.Subscribe(_deleteAndChargeObserver);
            _walletClient.Subscribe(_deleteAndChargeObserver);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDowm()
        {
            var deleteUsers = _registerUserObserver
                .GetAllUsers()
                .Except(_deleteAndChargeObserver.GetAllUsers());
            var tasks = deleteUsers
                .Select(userId => _userClient.DeleteUser(userId));

            await Task.WhenAll(tasks);
        }
    }
}
