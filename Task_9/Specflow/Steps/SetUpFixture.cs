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
        private static UserActionObserver _userActionObserver = new UserActionObserver();
        
        [ScenarioDependencies]
        public static ContainerBuilder ScenarioDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<TestDependencyModule>();
            return builder;
        }

        [BeforeScenario]
        public void BeforeScenario(UserServiceClient userClient, WalletServiceClient walletClient)
        {
            userClient.Subscribe(_userActionObserver);
            
            walletClient.Subscribe(_userActionObserver);
        }

        [AfterTestRun]
        public static async Task OneTimeTearDown(UserServiceClient userClient)
        {
            var deleteUsers = _userActionObserver
                .GetAllUsersToDelete();

            var tasks = deleteUsers
                .Select(userId => userClient.DeleteUser(userId));             
            
            await Task.WhenAll(tasks);
        }
    }
}