using Newtonsoft.Json;
using System.Collections.Generic;

namespace oht.Entities
{
    /// <summary>
    /// Word count data object returned by OHT API
    /// </summary>
    public class WordCount
    {
        /// <summary>
        /// Array of results per resource
        /// </summary>
        [JsonProperty(PropertyName = "resources")]
        public List<WordCountResource> Resources;

        /// <summary>
        /// Total words count
        /// </summary>
        [JsonProperty(PropertyName = "total")]
        public WordCountTotal Total;
    }


    /// <summary>
    /// Word count resource subobject
    /// </summary>
    public class WordCountResource
    {
        /// <summary>
        /// UUID of the resource in list
        /// </summary>
        [JsonProperty(PropertyName = "resource")]
        public string Resource { get; set; }

        /// <summary>
        /// total resource word count
        /// </summary>
        [JsonProperty(PropertyName = "wordcount")]
        public int Wordcount { get; set; }
    }

    /// <summary>
    /// Word count total subobject
    /// </summary>
    public class WordCountTotal
    {
        /// <summary>
        /// total words count
        /// </summary>
        [JsonProperty(PropertyName = "wordcount")]
        public int Wordcount { get; set; }
    }
}
