using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_9.Core.Clients;
using Task_9.Core.Contracts;
using Task_9.Core.Providers;
using static System.Formats.Asn1.AsnWriter;

namespace Task_9.Tests
{
    public abstract class BaseTest
    {
        protected readonly IUserServiceProvider _userProvider 
            = new UserServiceProvider(UserServiceClient.Instance, new Core.Utils.UserGenerator());
        protected readonly IWalletServiceProvider _walletProvider
            = new WalletServiceProvider(WalletServiceClient.Instance, new Core.Utils.BalanceChargeGenerator());
    }
}
