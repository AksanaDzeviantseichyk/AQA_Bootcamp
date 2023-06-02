using Newtonsoft.Json;

namespace Task_9.Core.Models.Requests
{
    public class BalanceChargeRequest
    {
        [JsonProperty("userId")]
        public int UserId;

        [JsonProperty("amount")]
        public decimal Amount;
    }
}
