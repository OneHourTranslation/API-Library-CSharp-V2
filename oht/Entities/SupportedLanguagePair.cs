using System.Collections.Generic;

using Newtonsoft.Json;


namespace oht.Entities
{
    /// <summary>
    /// Spported language pair data object returned by OHT API
    /// </summary>
    public class SupportedLanguagePair
    {
        /// <summary>
        /// Source language property
        /// </summary>
        [JsonProperty(PropertyName = "source")]
        public SupportedLanguage Source;

        /// <summary>
        /// List of target languages
        /// </summary>
        [JsonProperty(PropertyName = "targets")]
        public List<SupportedLanguagePairTarget> Targets;
    }
        
    /// <summary>
    /// Target language description data subobject
    /// </summary>
    public class SupportedLanguagePairTarget
    {
        /// <summary>
        /// Target language name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string LanguageName { get; set; }

        /// <summary>
        /// Target language code
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string LanguageCode { get; set; }

        /// <summary>
        /// Target language availability - high | medium | low Details:	high = work is expected to start within an hour on business hours medium = work is expected to start within one day	low = work is expected to start within a week
        /// </summary>
        [JsonProperty(PropertyName = "availability")]
        public LanguageAvailability Availability  { get; set; }
    }
}
