using Newtonsoft.Json;

namespace oht.Entities
{
    /// <summary>
    /// Basic account details and credits balance data object returned by API.   
    /// </summary>
    public class AccountDetails
    {
        /// <summary>
        /// Unique account id in OHT
        /// </summary>
        [JsonProperty(PropertyName = "account_id")]
        public int AccountId { get; set; }

        /// <summary>
        /// OHT username
        /// </summary>
        [JsonProperty(PropertyName = "account_username")]
        public string AccountUsername { get; set; }

        /// <summary>
        /// Currently available credits balance
        /// </summary>
        [JsonProperty(PropertyName = "credits")]
        public decimal Credits { get; set; }

        /// <summary>
        /// User Role
        /// </summary>
        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }

        /// <summary>
        /// Customer UUID
        /// </summary>
        [JsonProperty(PropertyName = "uuid")]
        public string Uuid { get; set; }
    }
}
