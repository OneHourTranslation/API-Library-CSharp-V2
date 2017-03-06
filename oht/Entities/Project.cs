using Newtonsoft.Json;

namespace oht.Entities
{
    /// <summary>
    /// Short project description data object returned by OHT API
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Project Id
        /// </summary>
        [JsonProperty(PropertyName = "project_id")]
        public int ProjectId { get; set; }

        /// <summary>
        /// Word count of the project
        /// </summary>
        [JsonProperty(PropertyName = "wordcount")]
        public int Wordcount { get; set; }

        /// <summary>
        /// Credits worth of the project
        /// </summary>
        [JsonProperty(PropertyName = "credits")]
        public decimal Credits { get; set; }
    }
}
