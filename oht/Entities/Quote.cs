using System.Collections.Generic;
using Newtonsoft.Json;

namespace oht.Entities
{   
    /// <summary>
    /// Quote data object returned by OHT API
    /// </summary>
    public class Quote
    {
        /// <summary>
        /// Currency selected by user (or default)
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public object Currency;
        
        /// <summary>
        /// Array of results per resource
        /// </summary>
        [JsonProperty(PropertyName = "resources")]
        public List<QuoteResource> Resources;
        
        /// <summary>
        /// Array of response params
        /// </summary>
        [JsonProperty(PropertyName = "total")]
        public QuoteTotal Total;
    }

    /// <summary>
    /// Quote resource subobject
    /// </summary>
    public class QuoteResource
    {
        /// <summary>
        /// Price of the resource
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }
        
        /// <summary>
        /// UUID of the resource in list
        /// </summary>
        [JsonProperty(PropertyName = "resource")]
        public string Resource { get; set; }
        
        /// <summary>
        /// Word count of the resource
        /// </summary>
        [JsonProperty(PropertyName = "wordcount")]
        public int Wordcount { get; set; }
        
        /// <summary>
        /// Credits worth of the resource
        /// </summary>
        [JsonProperty(PropertyName = "credits")]
        public decimal Credits { get; set; }
    }

    /// <summary>
    /// Quote total attributes subobject
    /// </summary>
    public class QuoteTotal
    {
        /// <summary>
        /// price in selected currency, based on credits and discounts
        /// </summary>
        [JsonProperty(PropertyName = "net_price")]
        public decimal NetPrice { get; set; }
        
        /// <summary>
        /// Price in selected currency, based on fee from payment vendors
        /// </summary>
        [JsonProperty(PropertyName = "transaction_fee")]
        public decimal TransactionFee { get; set; }
        
        /// <summary>
        /// Total price in selected currency, based on net price and transaction fee.
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }
        
        /// <summary>
        /// Total words count
        /// </summary>
        [JsonProperty(PropertyName = "wordcount")]
        public int Wordcount { get; set; }
        
        /// <summary>
        /// Sum of credits to charge
        /// </summary>
        [JsonProperty(PropertyName = "credits")]
        public decimal Credits { get; set; }
    }
}
