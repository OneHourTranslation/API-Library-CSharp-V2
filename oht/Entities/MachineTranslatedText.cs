using Newtonsoft.Json;

namespace oht.Entities
{
    /// <summary>
    /// Data object used to hold translated text and returned by OHT API. No need to expose it outside of the library, used internally only.
    /// </summary>
    internal class MachineTranslatedText
    {
        /// <summary>
        /// The translated content in the original format
        /// </summary>
        [JsonProperty(PropertyName = "TranslatedText")]
        public string TranslatedText { get; set; }
    }
}
