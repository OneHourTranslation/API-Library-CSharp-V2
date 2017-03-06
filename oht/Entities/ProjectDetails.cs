using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace oht.Entities
{   
    /// <summary>
    /// Project details data object returned by OHT API
    /// </summary>
    public class ProjectDetails
    {
        /// <summary>
        /// The unique id of the requested project
        /// </summary>
        [JsonProperty(PropertyName = "project_id")]
        public int ProjectId {get; set;}

        /// <summary>
        /// Translation | Expert Translation | Proofreading | Transcription | Translation + Proofreading |
        /// </summary>
        [JsonProperty(PropertyName = "project_type")]
        public ProjectType ProjectType {get; set;}

        /// <summary>
        /// Project status text representation
        /// </summary>
        [JsonProperty(PropertyName = "project_status")]
        public string ProjectStatus {get; set;}

        /// <summary>
        /// Pending | in_progress | submitted | signed | completed | canceled
        /// pending - project submitted to OHT, but professional worker (translator/proofreader) did not start working yet
        /// in_progress - worker started working on this project
        /// submitted - the worker uploaded the first target resource to the project. This does not mean that the project is completed.
        /// signed - the worker declared (with his signature) that he finished working on this project and all resources have been uploaded.
        /// completed - final state of the project, after which we cannot guarantee fixes or corrections. This state is automatically enforced after 4 days of inactivity on the project.
        /// </summary>
        [JsonProperty(PropertyName = "project_status_code")]
        public ProjectStatusCode ProjectStatusCode {get; set;}

        /// <summary>
        /// Source language code
        /// </summary>
        [JsonProperty(PropertyName = "source_language")]
        public string SourceLanguage {get; set;}

        /// <summary>
        /// Target language code
        /// </summary>
        [JsonProperty(PropertyName = "target_language")]
        public string TargetLanguage {get; set;}

        /// <summary>
        /// Resource UUID lists of the project's sources, translations, proofs and transcriptions
        /// </summary>
        [JsonProperty(PropertyName = "resources")]
        public ProjectDetailsResources Resources {get; set;}

        /// <summary>
        /// Project word count in case of transcription projects.
        /// </summary>
        [JsonProperty(PropertyName = "wordcount")]
        public int Wordcount {get; set;}

        /// <summary>
        /// Project length in seconds in case of transcription projects.
        /// </summary>
        [JsonProperty(PropertyName = "length")]
        public int Length {get; set;}

        /// <summary>
        /// This field has all custom fields if any were given.
        /// </summary>
        [JsonProperty(PropertyName = "custom")]
        public ProjectDetailsCustom Custom {get; set;}

        /// <summary>
        /// In case of multiple source files, this field will list all source files UUIDs and their corresponding translated files UUIDs.
        /// </summary>
        [JsonProperty(PropertyName = "resource_binding")]
        public JObject ResourceBindingRaw {get; set;}

        public List<ProjectResourceBinding> ResourceBinding { get; set; }

        /// <summary>
        /// Project linguist UUID.
        /// </summary>
        [JsonProperty(PropertyName = "linguist_uuid")]
        public string LinguistUuid {get; set;}

        /// <summary>
        /// List of all project tags. Empty if none was given.
        /// </summary>
        [JsonProperty(PropertyName = "tags")]
        public List<string> Tags {get; set;}

        /// <summary>
        /// In case of translation + editing project this field will show editing project ID. This field appears only after translation part of the project is completed.
        /// </summary>
        [JsonProperty(PropertyName = "following_project_id")]
        public string FollowingProject {get; set;}
    }

    /// <summary>
    /// Project details resource binding element holding from resource UUID and list of to resource UUIDs
    /// </summary>
    public class ProjectResourceBinding
    {
        public string FromResource { get; set; }
        public List<string> ToResources { get; set; }
    }

    /// <summary>
    /// Project details resources subobject contained by details object
    /// </summary>
    public class ProjectDetailsResources
    {
        /// <summary>
        /// List of source resource UUIDs related to the requested project
        /// </summary>
        [JsonProperty(PropertyName = "sources")]
        public List<string> Sources {get; set;}

        /// <summary>
        /// List of translation resource UUIDs related to the requested project
        /// </summary>
        [JsonProperty(PropertyName = "translations")]
        public List<string> Translations {get; set;}

        /// <summary>
        /// List of proofreading resource UUIDs related to the requested project
        /// </summary>
        [JsonProperty(PropertyName = "proofs")]
        public List<string> Proofs {get; set;}

        /// <summary>
        /// List of transcription resource UUIDs related to the requested project
        /// </summary>
        [JsonProperty(PropertyName = "transcriptions")]
        public List<string> Transcriptions {get; set;}

        /// <summary>
        /// This field returns an array of translated files UUIDs. It doesn't matter what kind of project was submitted, translated files UUIDs will be in this field.
        /// </summary>
        [JsonProperty(PropertyName = "results")]
        public List<string> TranslationResults {get; set;}

        /// <summary>
        /// In case reference files were provided in the project, this field will have an array of reference files UUIDs.
        /// </summary>
        [JsonProperty(PropertyName = "reference")]
        public List<string> Reference {get; set;}
    }

    /// <summary>
    /// Object representing custom fields inside main details object
    /// </summary>
    public class ProjectDetailsCustom
    {
        [JsonProperty(PropertyName = "api_custom_0")]
        public string Custom0 {get; set;}
        [JsonProperty(PropertyName = "api_custom_1")]
        public string Custom1 {get; set;}
        [JsonProperty(PropertyName = "api_custom_2")]
        public string Custom2 {get; set;}
        [JsonProperty(PropertyName = "api_custom_3")]
        public string Custom3 {get; set;}
        [JsonProperty(PropertyName = "api_custom_4")]
        public string Custom4 {get; set;}
        [JsonProperty(PropertyName = "api_custom_5")]
        public string Custom5 {get; set;}
        [JsonProperty(PropertyName = "api_custom_6")]
        public string Custom6 {get; set;}
        [JsonProperty(PropertyName = "api_custom_7")]
        public string Custom7 {get; set;}
        [JsonProperty(PropertyName = "api_custom_8")]
        public string Custom8 {get; set;}
        [JsonProperty(PropertyName = "api_custom_9")]
        public string Custom9 {get; set;}
    }
}
