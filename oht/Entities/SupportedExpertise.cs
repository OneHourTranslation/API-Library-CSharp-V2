using Newtonsoft.Json;

namespace oht.Entities
{
    /// <summary>
    /// Expertise data object returned by OHT API
    /// </summary>
    public class SupportedExpertise
    {
        /// <summary>
        /// Expertise name
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.AllowNull)]
        public string Name { get; set; }
        /// <summary>
        /// Expertise code
        /// </summary>
        [JsonProperty(PropertyName = "code", Required = Required.AllowNull)]
        public ExpertiseType Code { get; set; }

        /// <summary>
        /// Expertise identifier
        /// </summary>
        [JsonProperty(PropertyName = "expertise_id")]
        public int Id { get; set; }
    }
}
