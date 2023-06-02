using NUnit.Framework;
using Task_9.Core.Clients;
using Task_9.Core.Observers;

namespace Task_9.Tests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        private RegisterUserObserver _registerUserObserver;
        private DeleteAndChargeObserver _deleteAndChargeObserver;
        private UserServiceClient _userClient;
        private WalletServiceClient _walletClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _registerUserObserver = new RegisterUserObserver();
            _deleteAndChargeObserver = new DeleteAndChargeObserver();
            _userClient = UserServiceClient.Instance;
            _walletClient = WalletServiceClient.Instance;
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
