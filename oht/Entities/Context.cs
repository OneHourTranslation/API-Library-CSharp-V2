using Newtonsoft.Json;

namespace oht.Entities
{
    /// <summary>
    /// Context data object returned by OHT API
    /// </summary>
    public class Context
    {
        /// <summary>
        /// Unique context id in OHT
        /// </summary>
        [JsonProperty(PropertyName = "uuid")]
        public string Uuid { get; set; }

        /// <summary>
        /// Parent context (uuid) for the newly context, given by caller in the request
        /// </summary>
        [JsonProperty(PropertyName = "parent")]
        public string Parent { get; set; }

        /// <summary>
        /// Context's name given by caller in the request
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Context's ACL
        /// </summary>
        [JsonProperty(PropertyName = "acl")]
        public ContextACL Acl { get; set; }
    }

    /// <summary>
    /// Context details subobject holding ACL details
    /// </summary>
    public class ContextACL
    {
        /// <summary>
        /// Able to write
        /// </summary>
        [JsonProperty(PropertyName = "public_write")]
        public int PublicWrite { get; set; }

        /// <summary>
        /// Able to read
        /// </summary>
        [JsonProperty(PropertyName = "public_read")]
        public int PublicRead { get; set; }        
    }
}

