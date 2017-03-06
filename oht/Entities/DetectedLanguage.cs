using Newtonsoft.Json;

namespace oht.Entities
{
    /// <summary>
    /// Detected language data object returned by OHT API
    /// </summary>
    public class DetectedLanguage
    {
        /// <summary>
        /// Detected language code
        /// </summary>
        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }
    }
}

