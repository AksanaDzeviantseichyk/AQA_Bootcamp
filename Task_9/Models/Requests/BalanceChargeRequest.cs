using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_9.Models.Requests
{
    public class BalanceChargeRequest
    {
        [JsonProperty("userId")]
        public string UserId;

        [JsonProperty("amount")]
        public string Amount;
    }
}
