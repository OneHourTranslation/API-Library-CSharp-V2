using Newtonsoft.Json;
using System.Collections.Generic;

namespace oht.Restapi
{
    /// <summary>
    /// The class for all kind of responses returned by REST API
    /// </summary>
    /// <typeparam name="ResultType">Actual an valubale data returned by the call under results attribute, can be implemented as any kind of object</typeparam>
    internal class Response<ResultType> where ResultType : new()
    {
        [JsonProperty(PropertyName = "status")]
        public Status Status { get; set; }

        [JsonProperty(PropertyName = "errors", Required = Required.AllowNull)]
        public List<string> Errors;

        [JsonProperty(PropertyName = "results", Required = Required.AllowNull)]
        public ResultType Results { get; set; }
    }
}
