using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_9.Clients
{
    public class WalletServiceClient
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly string _baseUrl = "https://walletservice-uat.azurewebsites.net";

    }
}
