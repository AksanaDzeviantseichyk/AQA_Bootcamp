using Newtonsoft.Json;

namespace Task_9.Core.Models.Requests
{
    public class RegisterNewUserRequest
    {
        [JsonProperty("firstName")]
        public string? FirstName;

        [JsonProperty("lastName")]
        public string? LastName;
    }
}
