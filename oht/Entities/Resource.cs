using System;
using Newtonsoft.Json;

namespace oht.Entities
{
    /// <summary>
    /// Resource data object returned by OHT API
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// Type of resource. text|file
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public ResourceType Type { get; set; }

        /// <summary>
        /// File size in bytes
        /// </summary>
        [JsonProperty(PropertyName = "length")]
        public Int64 Length { get; set; }

        /// <summary>
        /// File name (only for files)
        /// </summary>
        [JsonProperty(PropertyName = "file_name")]
        public string FileName { get; set; }

        /// <summary>
        /// File mime (only for files)
        /// </summary>
        [JsonProperty(PropertyName = "file_mime")]
        public string FileMime { get; set; }

        /// <summary>
        /// URL to download as file (currently string link to the API call “Download resource”)
        /// </summary>
        [JsonProperty(PropertyName = "download_url")]
        public string DownloadUrl { get; set; }

        /// <summary>
        /// Base64 encoded (only if fetch=”base64”)
        /// </summary>
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
    }
}
