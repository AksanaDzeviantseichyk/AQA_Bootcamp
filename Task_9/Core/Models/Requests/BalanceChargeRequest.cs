using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_9.Core.Models.Requests
{
    public class BalanceChargeRequest
    {
        [JsonProperty("userId")]
        public Int32 UserId;

        [JsonProperty("amount")]
        public decimal Amount;
    }
}
