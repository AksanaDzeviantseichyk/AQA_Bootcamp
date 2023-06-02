using Autofac;
using Task_9.Core.Clients;
using Task_9.Core.Contracts;
using Task_9.Core.Modules;
using Task_9.Core.Providers;

namespace Task_9.Tests
{
    public abstract class BaseTest
    {
        protected readonly IUserServiceProvider _userProvider 
            = _scope.Resolve<IUserServiceProvider>();
        protected readonly IWalletServiceProvider _walletProvider 
            = _scope.Resolve<IWalletServiceProvider>();

        protected static ILifetimeScope _scope;

        static BaseTest()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<TestDependencyModule>();

            var container = builder.Build();

            _scope = container.BeginLifetimeScope();
        }


    }
}
