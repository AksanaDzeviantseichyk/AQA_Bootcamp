using Autofac;
using Task_9.Core.Clients;
using Task_9.Core.Contracts;
using Task_9.Core.Modules;
using Task_9.Core.Observers;
using Task_9.Core.Providers;

namespace Task_9.Tests
{
    public abstract class BaseTest
    {
        protected readonly IUserServiceProvider _userProvider = _scope.Resolve<IUserServiceProvider>();
        protected readonly IWalletServiceProvider _walletProvider = _scope.Resolve<IWalletServiceProvider>();
        protected readonly IUserServiceClient _userClient = _scope.Resolve<IUserServiceClient>();
        protected readonly IWalletServiceClient _walletClient = _scope.Resolve<IWalletServiceClient>();
        protected readonly RegisterUserObserver _registerUserObserver = _scope.Resolve<RegisterUserObserver>();
        protected readonly DeleteAndChargeObserver _deleteAndChargeObserver = _scope.Resolve<DeleteAndChargeObserver>();

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
