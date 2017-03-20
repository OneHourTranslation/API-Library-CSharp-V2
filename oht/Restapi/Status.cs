using Newtonsoft.Json;

namespace oht.Restapi
{
    /// <summary>
    /// REST API request call status description
    /// </summary>
    internal class Status
    {
        /// <summary>
        /// Request UUID
        /// </summary>
        [JsonProperty(PropertyName = "request_uuid")]
        public string RequestUuid { get; set; }

        /// <summary>
        /// Response status code, 0 if success
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        /// <summary>
        /// Response status message, "ok" is success
        /// </summary>
        [JsonProperty(PropertyName = "msg")]
        public string Message { get; set; }
    }
}
