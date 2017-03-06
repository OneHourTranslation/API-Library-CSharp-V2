using Newtonsoft.Json;

namespace oht.Entities
{
    /// <summary>
    /// Project comment data object returned by OHT API
    /// </summary>
    public class ProjectComment
    {
        /// <summary>
        /// Unique id of the comment
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int Id;

        /// <summary>
        /// The date the comment was created
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public string Date;

        /// <summary>
        /// Short representation of the user’s name
        /// </summary>
        [JsonProperty(PropertyName = "commenter_name")]
        public string CommenterName;

        /// <summary>
        /// admin | customer | provider | potential-provider
        /// admin - OHT support team
        /// provider - The translator / proofreader / transcriber that is assigned to the project
        /// potential-provider - A translator / proofreader / transcriber that was allowed to view the project before it was assigned
        /// </summary>
        [JsonProperty(PropertyName = "commenter_role")]
        public CommenterRole CommenterRole;

        /// <summary>
        /// Text content
        /// </summary>
        [JsonProperty(PropertyName = "comment_content")]
        public string CommentContent;
    }
}
