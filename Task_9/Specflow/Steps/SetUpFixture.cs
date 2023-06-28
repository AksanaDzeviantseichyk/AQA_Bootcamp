using Autofac;
using SpecFlow.Autofac;
using Task_9.Core.Contracts;
using Task_9.Core.Modules;
using Task_9.Core.Observers;
using TechTalk.SpecFlow;

namespace Task_9.Specflow.Steps
{
    [Binding]
    public class SetUpFixture
    {
        private static  IUserServiceClient _userClient;
        private static  IWalletServiceClient _walletClient;
        private static  RegisterUserObserver _registerUserObserver;
        private static  DeleteAndChargeObserver _deleteAndChargeObserver;
        private static UserObservers _userObservers;
        
        [ScenarioDependencies]
        public static ContainerBuilder ScenarioDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<TestDependencyModule>();
            return builder;
        }

        [BeforeTestRun]
        public static void OneTimeSetUp()
        {
            var container = ScenarioDependencies().Build();
            _userClient = container.Resolve<IUserServiceClient>();
            _walletClient = container.Resolve<IWalletServiceClient>();
            _registerUserObserver = container.Resolve<RegisterUserObserver>();
            _deleteAndChargeObserver = container.Resolve<DeleteAndChargeObserver>();
            _userObservers = container.Resolve<UserObservers>();
                
            _userClient.Subscribe(_registerUserObserver);
            _userClient.Subscribe(_deleteAndChargeObserver);
            _walletClient.Subscribe(_deleteAndChargeObserver);

        }

        [AfterTestRun]
        public static async Task OneTimeTearDown()
        {
            var registerUserObserver = _userObservers.GetRegisterUserObserver();
            var deleteAndChargeObserver = _userObservers.GetDeleteAndChargeObserver();
                        
            var deleteUsers = registerUserObserver
                .GetAllUsers()
                .Except(deleteAndChargeObserver
                .GetAllUsers());

            var tasks = deleteUsers
                .Select(userId => _userClient.DeleteUser(userId));             
            
            await Task.WhenAll(tasks);
        }
    }
}