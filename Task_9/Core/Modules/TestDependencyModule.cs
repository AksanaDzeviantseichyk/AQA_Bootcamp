using Autofac;
using Task_9.Core.Clients;
using Task_9.Core.Contracts;
using Task_9.Core.Providers;
using Task_9.Core.Utils;

namespace Task_9.Core.Modules
{
    public class TestDependencyModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<UserGenerator>()
                .AsSelf();

            builder
                .RegisterType<UserServiceClient>()
                .As<IUserServiceClient>()
                .AsSelf();

            builder
                .RegisterType<UserServiceProvider>()
                .As<IUserServiceProvider>()
                .AsSelf();

            builder
                .RegisterType<BalanceChargeGenerator>()
                .AsSelf();

            builder
                .RegisterType<WalletServiceClient>()
                .As<IWalletServiceClient>()
                .AsSelf();

            builder
                .RegisterType<WalletServiceProvider>()
                .As<IWalletServiceProvider>()
                .AsSelf();
        }


    }
}
