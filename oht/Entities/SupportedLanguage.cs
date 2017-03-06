using Newtonsoft.Json;

namespace oht.Entities
{
    /// <summary>
    /// Supported language data object returned by OHT API
    /// </summary>
    public class SupportedLanguage
    {
        /// <summary>
        /// Language name (e.g. English, French etc).
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string LanguageName { get;  set;}

        /// <summary>
        /// Language Code
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string LanguageCode { get; set; }
    }
}
