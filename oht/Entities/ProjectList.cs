using System.Collections.Generic;
using Newtonsoft.Json;

namespace oht.Entities
{
    /// <summary>
    /// Project list data object returned by OHT API
    /// </summary>
    public class ProjectList
    {
        /// <summary>
        /// Total project count returned by the search
        /// </summary>
        [JsonProperty(PropertyName = "projectsCount")]
        public int projectsCount { get; set; }

        /// <summary>
        /// Array of projects.
        /// </summary>
        [JsonProperty(PropertyName = "projects")]
        public ProjectItem[] Projects { get; set; }
    }

    /// <summary>
    /// Project item details subobject returned by the search
    /// </summary>
    public class ProjectItem
    {
        /// <summary>
        /// Project ID.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Project ID prefix, added to each project.
        /// </summary>
        [JsonProperty(PropertyName = "id_prefix")]
        public string IdPrefix { get; set; }

        /// <summary>
        /// Project name.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Project type (translation | transcription | proofreading | combo_translation_proofreading)
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Language pair
        /// </summary>
        [JsonProperty(PropertyName = "languagePair")]
        public ProjectLanguagePair LanguagePair { get; set; }

        /// <summary>
        /// Time stamp of project creation date.
        /// </summary>
        [JsonProperty(PropertyName = "created")]
        public int Created { get; set; }

        /// <summary>
        /// Project volume.
        /// </summary>
        [JsonProperty(PropertyName = "volume")]
        public ProjectVolume Volume { get; set; }

        /// <summary>
        /// Estimated translation time.
        /// </summary>
        [JsonProperty(PropertyName = "estimation")]
        public int Estimation { get; set; }

        /// <summary>
        /// Once the projects is being translated this field will return a time stamp of project deadline.
        /// </summary>
        [JsonProperty(PropertyName = "expiration")]
        public bool Expiration { get; set; }

        /// <summary>
        /// Expertise details, name and code
        /// </summary>
        [JsonProperty(PropertyName = "expertise")]
        public SupportedExpertise Expertise { get; set; }

        /// <summary>
        /// Current project status.
        /// </summary>
        [JsonProperty(PropertyName = "status_summary")]
        public string StatusSummary { get; set; }

        /// <summary>
        /// In case of translation + editing project, this field will return editing project id
        /// </summary>
        [JsonProperty(PropertyName = "child_project")]
        public object ChildProjects { get; set; }

        /// <summary>
        /// List of project tags
        /// </summary>
        [JsonProperty(PropertyName = "tags")]
        public List<ProjectTag> Tags { get; set; }

        /// <summary>
        /// This field has all custom fields if any were given.
        /// </summary>
        [JsonProperty(PropertyName = "custom_fields")]
        public object[] Custom { get; set; }
    }

    /// <summary>
    /// Project language pair data subobject
    /// </summary>
    public class ProjectLanguagePair
    {
        /// <summary>
        /// Source language name and code.
        /// </summary>
        [JsonProperty(PropertyName = "source_language")]
        public SupportedLanguage SourceLanguage { get; set; }

        /// <summary>
        /// Target language name and code.
        /// </summary>
        [JsonProperty(PropertyName = "target_language")]
        public SupportedLanguage TargetLanguage { get; set; }
    }

    /// <summary>
    /// Project volumn data subobject
    /// </summary>
    public class ProjectVolume
    {
        /// <summary>
        /// Measure - word | page
        /// </summary>
        [JsonProperty(PropertyName = "unit")]
        public string Unit { get; set; }

        /// <summary>
        /// Number of words/pages.
        /// </summary>
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
    }

    /// <summary>
    /// Project tag data subobject
    /// </summary>
    public class ProjectTag
    {
        /// <summary>
        /// Tag content
        /// </summary>
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }
    }
}
