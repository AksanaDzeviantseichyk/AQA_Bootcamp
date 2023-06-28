using Autofac;
using SpecFlow.Autofac;
using System.Collections.Concurrent;
using Task_9.Core.Clients;
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
        private static IContainer _container;


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
            var builder = ScenarioDependencies();
            _container = builder.Build();
            _userClient = _container.Resolve<IUserServiceClient>();
            _walletClient = _container.Resolve<IWalletServiceClient>();
            _registerUserObserver = _container.Resolve<RegisterUserObserver>();
            _deleteAndChargeObserver = _container.Resolve<DeleteAndChargeObserver>();
                        
            _userClient.Subscribe(_registerUserObserver);
            _userClient.Subscribe(_deleteAndChargeObserver);
            _walletClient.Subscribe(_deleteAndChargeObserver);
        }

        [AfterTestRun]
        public static async Task OneTimeTearDown()
        {
            
            var deleteUsers = _registerUserObserver
                .GetAllUsers()
                .Except(_deleteAndChargeObserver
                .GetAllUsers());

            var tasks = deleteUsers
                .Select(userId => _userClient.DeleteUser(userId));             
            
            await Task.WhenAll(tasks);
        }
    }
}