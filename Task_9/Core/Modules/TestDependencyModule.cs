using Autofac;
using Task_9.Core.Clients;
using Task_9.Core.Contracts;
using Task_9.Core.Observers;
using Task_9.Core.Providers;
using Task_9.Core.Utils;
using Task_9.Specflow;
using Task_9.Specflow.Steps;

namespace Task_9.Core.Modules
{
    public class TestDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<UserGenerator>()
                .AsSelf();

            builder
                .RegisterType<UserServiceClient>()
                .As<IUserServiceClient>()
                .SingleInstance()
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
                .SingleInstance()
                .AsSelf();

            builder
                .RegisterType<WalletServiceProvider>()
                .As<IWalletServiceProvider>()
                .AsSelf();

            builder
                .RegisterType<UserActionObserver>()
                .SingleInstance()
                .AsSelf();  
            
            builder
               .RegisterType<UserServiceSteps>()
               .AsSelf();

            builder
                .RegisterType<UserServiceAssertSteps>()
                .AsSelf();

            builder
               .RegisterType<WalletServiceSteps>()
               .AsSelf();

            builder
                .RegisterType<WalletServiceAssertSteps>()
                .AsSelf();

            builder
               .RegisterType<UserDataContext>()
               .SingleInstance()
               .AsSelf();

            builder
               .RegisterType<WalletDataContext>()
               .SingleInstance()
               .AsSelf();
            builder
                .RegisterType<SetUpFixture>();
        }
    }
}
