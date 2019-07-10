using System;
using System.Net;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using oht.Entities;
using oht.Restapi;

namespace oht
{
    /// <summary>
    /// Main OneHourTranslate API class
    /// </summary>
    public class OhtApi
    {
        #region constants and variables
        private static string _baseUrl = "https://www.onehourtranslation.com/api/2";
        private static string _baseUrlSandbox = "https://sandbox.onehourtranslation.com/api/2";
        private Request _request;
        #endregion

        #region constructors
        public OhtApi(string publicKey) : this(null, publicKey, false, null)
        {
        }

        public OhtApi(string publicKey, bool useSandbox) : this(null, publicKey, useSandbox, null)
        {
        }

        public OhtApi(string publicKey, bool useSandbox, WebProxy proxy) : this(null, publicKey, useSandbox, proxy)
        {
        }

        public OhtApi(string secretKey, string publicKey) : this(secretKey, publicKey, false, null)
        {
        }

        public OhtApi(string secretKey, string publicKey, bool useSandbox) : this(secretKey, publicKey, useSandbox, null)
        {
        }

        public OhtApi(string secretKey, string publicKey, bool useSandbox, WebProxy proxy)
        {
            _request = new Request(publicKey, secretKey, (useSandbox ? _baseUrlSandbox : _baseUrl), proxy);
        }

        public OhtApi(string secretKey, string publicKey, string baseUrl, WebProxy proxy)
        {
            _request = new Request(publicKey, secretKey, baseUrl, proxy);
        }
        #endregion

        #region public methods
        /// <summary>
        /// GET ACCOUNT DETAILS
        /// Fetch basic account details and credits balance
        /// GET: /account
        /// </summary>
        /// <returns>Account details object</returns>
        public AccountDetails GetAccountDetails()
        {
            var result = _request.Get<AccountDetails>("/account");
            return result.Results;
        }

        /// <summary>
        /// CREATE FILE RESOURCE
        /// Create a new file entity on One Hour Translation. After the resource entity is created, it can be used on job requests such as translation, proofreading, etc.
        /// POST: /resources/file
        /// </summary>
        /// <param name="upload">File content to upload, submitted via multipart/form-data request.</param>
        /// <param name="fileName">(optional) Replace the original file's name on One Hour Translation.</param>
        /// <param name="fileMime">(optional) Replace the default mime value for the file.</param>
        /// <param name="filePath">(optional) Full file path to upload, used if upload parameter is empty.</param>
        /// <returns>Resource UUID</returns>
        public string CreateFileResource(string upload, string fileName = "", string fileMime = "", string filePath = "")
        {
            var parameters = new List<Param>();
            if (!string.IsNullOrWhiteSpace(upload))
            {
                parameters.Add(new Param("upload", upload, ParamType.FileContent));
            }
            else if (!string.IsNullOrWhiteSpace(filePath))
            {
                parameters.Add(new Param("upload", filePath, ParamType.FilePath));
            }

            if (!string.IsNullOrWhiteSpace(fileName))
                parameters.Add(new Param("file_name", fileName));

            if (!string.IsNullOrWhiteSpace(fileMime))
                parameters.Add(new Param("file_mime", fileMime));

            var result = _request.Post<List<string>>("/resources/file", parameters.ToArray());
            return result.Results[0];
        }

        /// <summary>
        /// CREATE TEXT RESOURCE
        /// Create a new text entity on One Hour Translation.
        /// POST: /resources/text
        /// </summary>
        /// <param name="text">Actual text that will be later used as source text for translation, proofreading or other services. Accepts Unicode charset only.</param>
        /// <returns>Resource UUID</returns>
        public string CreateTextResource(string text)
        {
            var result = _request.Post<List<string>>("/resources/text", new Param("text", text));
            return result.Results[0];
        }

        /// <summary>
        /// GET RESOURCE
        /// Fetch resource information.
        /// GET: /resources/<resource_uuid>
        /// </summary>
        /// <param name="resourceUuid">Resource UUID</param>
        /// <param name="projectId">(optional) Project ID, needed when requesting a resource that was uploaded by another user - e.g. as a project’s translation</param>
        /// <param name="fetch">(optional) possible values: false - (default) do not fetch content ; base64 - fetch content, base64 encoded</param>
        /// <returns>Resource details object</returns>
        public Resource GetResource(string resourceUuid, int projectId = 0, string fetch = "false")
        {
            var parameters = new List<Param>();
            parameters.Add(new Param("fetch", fetch));

            if (projectId > 0)
                parameters.Add(new Param("project_id", projectId));

            var result = _request.Get<Resource>("/resources/" + resourceUuid, parameters.ToArray());
            return result.Results;
        }

        /// <summary>
        /// DOWNLOAD RESOURCE
        /// Return a “file download” response, not JSON.
        /// GET: /resources/<resource_uuid>/download
        /// </summary>
        /// <param name="resourceUuid">UUID of the desired resource.</param>
        /// <param name="filePath">File path to save content locally.</param>
        /// <param name="projectId">(optional) Project ID, needed when requesting a resource that was uploaded by another user - e.g. as a project’s translation.</param>
        public void DownloadResource(string resourceUuid, string filePath, int projectId = 0)
        {
            var parameters = new List<Param>();
            parameters.Add(new Param("file_path", filePath, ParamType.FilePath));
            if (projectId > 0)
                parameters.Add(new Param("project_id", projectId));

            _request.Get<List<string>>("/resources/" + resourceUuid + "/download", parameters.ToArray());
        }

        /// <summary>
        /// SUPPORTED LANGUAGES
        /// GET: /discover/languages
        /// </summary>
        /// <returns>List of language details objects.</returns>
        public List<SupportedLanguage> GetSupportedLanguages()
        {
            var result = _request.Get<List<SupportedLanguage>>("/discover/languages");
            return result.Results;
        }

        /// <summary>
        /// SUPPORTED LANGUAGE PAIRS
        /// GET: /discover/language_pairs
        /// </summary>
        /// <returns>List of supported lnaguage pair objects.</returns>
        public List<SupportedLanguagePair> GetSupportedLanguagePairs()
        {
            var result = _request.Get<List<SupportedLanguagePair>>("/discover/language_pairs");
            return result.Results;
        }

        /// <summary>
        /// DETECT LANGUAGE
        /// Detect language of your text.
        /// GET: /mt/detect/text
        /// </summary>
        /// <param name="sourceContent">Text for language detection.</param>
        /// <returns>Detected language details object.</returns>
        public DetectedLanguage DetectLanguage(string sourceContent)
        {
            var result = _request.Get<DetectedLanguage>("/mt/detect/text", new Param("source_content", sourceContent));
            return result.Results;
        }

        /// <summary>
        /// SUPPORTED EXPERTISE
        /// GET: /discover/expertise
        /// </summary>
        /// <param name="sourceLanguage">(optional, mandatory if target_language is specified) Source language code.</param>
        /// <param name="targetLanguage">(optional, mandatory if source_language is specified) Target language code.</param>
        /// <returns>List of supported expertise details objects.</returns>
        public List<SupportedExpertise> GetSupportedExpertises(string sourceLanguage = "", string targetLanguage = "")
        {
            var parameters = new List<Param>();
            if (!string.IsNullOrWhiteSpace(sourceLanguage))
                parameters.Add(new Param("source_language", sourceLanguage));
            if (!string.IsNullOrWhiteSpace(targetLanguage))
                parameters.Add(new Param("target_language", targetLanguage));

            var result = _request.Get<List<SupportedExpertise>>("/discover/expertise", parameters.ToArray());
            return result.Results;
        }

        /// <summary>
        /// TRANSLATE
        /// Get machine translation.
        /// GET: /mt/translate/text
        /// </summary>
        /// <param name="sourceLanguage">Source language code.</param>
        /// <param name="targetLanguage">Target language code.</param>
        /// <param name="sourceContent">Text to translate.</param>
        /// <returns>Translated text.</returns>
        public string MachineTranslation(string sourceLanguage, string targetLanguage, string sourceContent)
        {
            var result = _request.Get<MachineTranslatedText>("/mt/translate/text", new Param("source_language", sourceLanguage), new Param("target_language", targetLanguage), new Param("source_content", sourceContent));
            return result.Results.TranslatedText;
        }

        /// <summary>
        /// GET WORD COUNT
        /// This call will return the number of words in each attached resource and total number of words in all resources.
        /// GET: /tools/wordcount
        /// </summary>
        /// <param name="resources">List of resource_uuids.</param>
        /// <returns>Word count details object.</returns>
        public WordCount GetWordCount(List<string> resources)
        {
            var result = _request.Get<WordCount>("/tools/wordcount", new Param("resources", string.Join<string>(",", resources)));
            return result.Results;
        }

        /// <summary>
        /// GET QUOTE
        /// Get a quote from OneHourTranslation. This call will generate a quote for your project depending on your needs.
        /// GET: /tools/quote
        /// </summary>
        /// <param name="resources">List of resource_uuid. This field is mandatory if "wordcount" not specified.</param>
        /// <param name="wordCount">Integer, words count.</param>
        /// <param name="sourceLanguage">Source language code.</param>
        /// <param name="targetLanguage">Target language code.</param>
        /// <param name="service">(optional, default is translation) Possible values: translation | proofreading | transproof | transcription (defaults to translation)</param>
        /// <param name="expertise">(optional) Expertise code</param>
        /// <param name="proofReading">(optional, use only if "service" isn't specified) 0 | 1</param>
        /// <param name="currency">(optional) USD | EUR | GBP</param>
        /// <returns>Quote details object</returns>
		public Quote GetQuote(List<string> resources, int wordCount, string sourceLanguage, string targetLanguage, ServiceType service = ServiceType.None,
            ExpertiseType expertise = ExpertiseType.None, ProofReading proofReading = ProofReading.None, CurrencyType currency = CurrencyType.None)
        {
            var parameters = new List<Param>();
            parameters.Add(new Param("resources", string.Join<string>(",", resources)));
            parameters.Add(new Param("wordcount", wordCount));
            parameters.Add(new Param("source_language", sourceLanguage));
            parameters.Add(new Param("target_language", targetLanguage));
            if (service != ServiceType.None)
                parameters.Add(new Param("service", service.GetStringValue()));
            if (expertise != ExpertiseType.None)
                parameters.Add(new Param("expertise", expertise.GetStringValue()));
            if (proofReading != ProofReading.None)
                parameters.Add(new Param("proofreading", proofReading.GetStringValue()));
            if (currency != CurrencyType.None)
                parameters.Add(new Param("currency", currency.GetStringValue()));

            var result = _request.Get<Quote>("/tools/quote", parameters.ToArray());
            return result.Results;
        }

        /// <summary>
        /// CREATE TRANSLATION PROJECT
        /// Create new translation project.
        /// POST: /projects/translation
        /// </summary>
        /// <param name="sourceLanguage">Source language code.</param>
        /// <param name="targetLanguage">Target language code.</param>
        /// <param name="sources">List of Resource UUIDs. You can create a project with up to 30 resources at a time. For more than 30 resources please create a new project.</param>
        /// <param name="refResources">(optional) List of reference resource UUIDs.</param>
        /// <param name="wordCount">(optional) Integer, if empty use automatic counting.</param>
        /// <param name="notes">(optional) Text note that will be shown to translator regarding the new project.</param>
        /// <param name="expertise">(optional) Expertise code.</param>
        /// <param name="name">(optional) Name your project. If empty, your project will be named automatically.</param>
        /// <param name="callbackUrl">(optional) Callback URL.</param>
        /// <param name="custom">(optional) Custom callback parameters [0...9]</param>
        /// <returns>Project details object</returns>
        public Project CreateTranslationProject(string sourceLanguage, string targetLanguage, List<string> sources, List<string> refResources = null,
            int wordCount = 0, string notes = "", ExpertiseType expertise = ExpertiseType.None, string name = "", string callbackUrl = "", params string[] custom)
        {
            return CreateProject("/projects/translation", sourceLanguage, targetLanguage, sources, null, refResources, wordCount, notes, expertise, name, callbackUrl, custom);
        }

        /// <summary>
        /// CREATE TRANSLATION + EDITING
        /// Create new translation + editing project.
        /// POST: /projects/transproof
        /// </summary>
        /// <param name="sourceLanguage">Source language code.</param>
        /// <param name="targetLanguage">Target language code.</param>
        /// <param name="sources">List of Resource UUIDs. You can create a project with up to 30 resources at a time. For more than 30 resources please create a new project.</param>
        /// <param name="refResources">(optional) List of reference resource UUIDs.</param>
        /// <param name="wordCount">(optional) Integer, if empty use automatic counting.</param>
        /// <param name="notes">(optional) Text note that will be shown to translator regarding the new project.</param>
        /// <param name="expertise">(optional) Expertise code.</param>
        /// <param name="name">(optional) Name your project. If empty, your project will be named automatically.</param>
        /// <param name="callbackUrl">(optional) Callback URL.</param>
        /// <param name="custom">(optional) Custom callback parameters [0...9]</param>
        /// <returns>Project details object</returns>
        public Project CreateTransProofProject(string sourceLanguage, string targetLanguage, List<string> sources, List<string> refResources = null,
            int wordCount = 0, string notes = "", ExpertiseType expertise = ExpertiseType.None, string name = "", string callbackUrl = "", params string[] custom)
        {
            return CreateProject("/projects/transproof", sourceLanguage, targetLanguage, sources, null, refResources, wordCount, notes, expertise, name, callbackUrl, custom);
        }

        /// <summary>
        /// CREATE PROOFREADING PROJECT (SOURCE)
        /// Create new proofreading project(source language only).
        /// POST: /projects/proof-general        
        /// </summary>
        /// <param name="sourceLanguage">Source language code.</param>        
        /// <param name="sources">List of Resource UUIDs. You can create a project with up to 30 resources at a time. For more than 30 resources please create a new project.</param>
        /// <param name="refResources">(optional) List of reference resource UUIDs.</param>
        /// <param name="wordCount">(optional) Integer, if empty use automatic counting.</param>
        /// <param name="notes">(optional) Text note that will be shown to translator regarding the new project.</param>
        /// <param name="name">(optional) Name your project. If empty, your project will be named automatically.</param>
        /// <param name="callbackUrl">(optional) Callback URL.</param>
        /// <param name="custom">(optional) Custom callback parameters [0...9]</param>
        /// <returns>Project details object</returns>
        public Project CreateProofreadingProject(string sourceLanguage, List<string> sources, List<string> refResources = null,
            int wordCount = 0, string notes = "", string name = "", string callbackUrl = "", params string[] custom)
        {
            return CreateProject("/projects/proof-general", sourceLanguage, "", sources, null, refResources, wordCount, notes, ExpertiseType.None, name, callbackUrl, custom);
        }

        /// <summary>
        /// CREATE PROOFREADING PROJECT (SOURCE AND TARGET)
        /// Create new proofreading project(source and target).
        /// POST: /projects/proof-translated
        /// </summary>
        /// <param name="sourceLanguage">Source language code.</param>
        /// <param name="targetLanguage">Target language code.</param>
        /// <param name="sources">List of Resource UUIDs. You can create a project with up to 30 resources at a time. For more than 30 resources please create a new project.</param>
        /// <param name="translations">List of translation resource UUIDs.</param>
        /// <param name="refResources">(optional) List of reference resource UUIDs.</param>
        /// <param name="wordCount">(optional) Integer, if empty use automatic counting.</param>
        /// <param name="notes">(optional) Text note that will be shown to translator regarding the new project.</param>
        /// <param name="name">(optional) Name your project. If empty, your project will be named automatically.</param>
        /// <param name="callbackUrl">(optional) Callback URL.</param>
        /// <param name="custom">(optional) Custom callback parameters [0...9]</param>
        /// <returns>Project details object</returns>
        public Project CreateProofTranslatedProject(string sourceLanguage, string targetLanguage, List<string> sources, List<string> translations,
            List<string> refResources = null, int wordCount = 0, string notes = "", ExpertiseType expertise = ExpertiseType.None, string name = "", string callbackUrl = "", params string[] custom)
        {
            return CreateProject("/projects/proof-translated", sourceLanguage, targetLanguage, sources, translations, refResources, wordCount, notes, ExpertiseType.None, name, callbackUrl, custom);
        }

        /// <summary>
        /// CREATE TRANSCRIPTION PROJECT
        /// Create new transcription project.
        /// POST: /projects/transcription
        /// </summary>
        /// <param name="sourceLanguage">Source language code.</param>        
        /// <param name="sources">List of Resource UUIDs. You can create a project with up to 30 resources at a time. For more than 30 resources please create a new project.</param>
        /// <param name="refResources">(optional) List of reference resource UUIDs.</param>
        /// <param name="length">(optional) Integer of seconds, if empty use automatic counting.</param>
        /// <param name="notes">(optional) Text note that will be shown to translator regarding the new project.</param>
        /// <param name="name">(optional) Name your project. If empty, your project will be named automatically.</param>
        /// <param name="callbackUrl">(optional) Callback URL.</param>
        /// <param name="custom">(optional) Custom callback parameters [0...9]</param>
        /// <returns>Project details object</returns>
        public Project CreateTranscriptionProject(string sourceLanguage, List<string> sources, List<string> refResources = null,
            int wordCount = 0, string notes = "", string name = "", string callbackUrl = "", params string[] custom)
        {
            return CreateProject("/projects/transcription", sourceLanguage, "", sources, null, refResources, wordCount, notes, ExpertiseType.None, name, callbackUrl, custom);
        }

        /// <summary>
        /// CANCEL PROJECT
        /// Cancel one of your projects.
        /// Constraints: Available only before actual translation starts
        /// DELETE: /projects/<project_id>
        /// </summary>
        /// <param name="projectId">Project ID.</param>
        public void CancelProject(int projectId)
        {
            _request.Delete<List<string>>(string.Format("/projects/{0}", projectId));
        }

        /// <summary>
        /// GET PROJECT DETAILS
        /// Fetch project information.
        /// GET: /projects/<project_id>
        /// </summary>
        /// <param name="projectId">Project ID.</param>
        /// <returns>Project details object.</returns>
        public ProjectDetails GetProjectDetails(int projectId)
        {
            var result = _request.Get<ProjectDetails>(string.Format("/projects/{0}", projectId));
            ParseProjectResourceBinding(result.Results);
            return result.Results;
        }

        /// <summary>
        /// GET PROJECTS LIST
        /// Fetch all user projects.
        /// GET: /account/projects
        /// </summary>
        /// <param name="limit">(Optional) Limit the amount of projects returned. Default is set to 100 if no limit is provided.</param>
        /// <param name="page">(Optional) Each page will list the amount of projects specified by the limit parameter. Default is 0 which will return the first set of projects.</param>
        /// <param name="sortBy">(Optional) id | type | due | expertise (Default is set to "id").</param>
        /// <param name="sortDirection">(Optional) asc - Ascending direction. desc - Descending direction. Default is set to descending direction.</param>
        /// <param name="filterByStatus">(Optional) CANCELLED | IN_PROGRESS | COMPLETED If no value was provided will return all projects.</param>
        /// <param name="filterByDateFrom">(Optional) Time stamp, call will return all projects that were created after this date.</param>
        /// <param name="filterByDateTo">(Optional) Time stamp, call will return all projects created before this date.</param>
        /// <returns></returns>
        public ProjectList GetProjectsList(int limit = 100, int page = 0, string sortBy = "Id", string sortDirection = "desc", string filterByStatus = "", int filterByDateFrom = 0, int filterByDateTo = 0)
        {
            var parameters = new List<Param>();
            if (limit != 100)
                parameters.Add(new Param("limit", limit));
            if (page > 0)
                parameters.Add(new Param("page", page));
            if (!string.IsNullOrWhiteSpace(sortBy))
                parameters.Add(new Param("sortBy", sortBy));
            if (!string.IsNullOrWhiteSpace(sortDirection))
                parameters.Add(new Param("sortDirection", sortDirection));
            if (!string.IsNullOrWhiteSpace(filterByStatus))
                parameters.Add(new Param("filterByStatus", filterByStatus));
            if (filterByDateFrom > 0)
                parameters.Add(new Param("filterByDateFrom", filterByDateFrom));
            if (filterByDateTo > 0)
                parameters.Add(new Param("filterByDateTo", filterByDateTo));

            var result = _request.Get<ProjectList>("/account/projects", parameters.ToArray());
            return result.Results;
        }

        /// <summary>
        /// COMMENTS > NEW COMMENT
        /// Add new comment to a project.
        /// POST: /projects/<project_id>/comments
        /// </summary>
        /// <param name="projectId">Project ID.</param>
        /// <param name="content">Text content.</param>
        public void AddProjectComment(int projectId, string content)
        {
            _request.Post<List<string>>(string.Format("/projects/{0}/comments", projectId), new Param("content", content));
        }

        /// <summary>
        /// COMMENTS > GET COMMENTS
        /// Fetch project comments.
        /// GET: /projects/<project_id>/comments
        /// </summary>
        /// <param name="projectId">Project ID.</param>
        /// <returns>List of project comment objects.</returns>
        public List<ProjectComment> GetProjectComments(int projectId)
        {
            var result = _request.Get<List<ProjectComment>>(string.Format("/projects/{0}/comments", projectId));
            return result.Results;
        }


        /// <summary>
        /// ADD NEW TAG TO PROJECT    
        /// POST: /project/<project_id>/tag
        /// </summary>
        /// <param name="projectId">Project ID.</param>
        /// <param name="tagName">String, your tag. You can add one tag each call.</param>
        public void AddProjectTag(int projectId, string tagName)
        {
            _request.Post<List<string>>(string.Format("/project/{0}/tag", projectId), new Param("tag_name", tagName));
        }

        /// <summary>
        /// DELETE TAG FROM PROJECT
        /// DELETE: /project/<project_id>/tag/<tag_id>
        /// </summary>
        /// <param name="projectId">Project ID.</param>
        /// <param name="tagId">Integer, tag id. See "Get project tags" API call.</param>
        public void DeleteProjectTag(int projectId, int tagId)
        {
            _request.Delete<List<string>>(string.Format("/project/{0}/tag/{1}", projectId, tagId));
        }

        /// <summary>
        /// GET PROJECT TAGS
        /// Fetch project tags.
        /// GET: /project/<project_id>/tag
        /// </summary>
        /// <param name="projectId">Project ID.</param>
        /// <returns>Dictionary of tag Ids and Names</returns>
        public Dictionary<int, string> GetProjectTags(int projectId)
        {
            var response = _request.Get<JObject>(string.Format("/project/{0}/tag", projectId));
            Dictionary<int, string> result = null;
            if (response.Results != null && response.Results.HasValues)
            {
                result = new Dictionary<int, string>();
                var tag = response.Results.First;
                while (tag != null)
                {
                    int id = -1;
                    Int32.TryParse(tag.Path, out id);
                    if (id > -1)
                    {
                        if (tag.HasValues)
                        {
                            result.Add(id, tag.First.ToString());                            
                        }
                    }
                    tag = tag.Next;
                }
            }

            return result;
        }

        /// <summary>
        /// RATE PROJECT
        /// Rate translation and overall customer experience.
        /// POST: /projects/<project_id>/rating
        /// </summary>
        /// <param name="projectId">Project ID.</param>
        /// <param name="type">You can rate one service at a time - customer | service.
        /// customer - Rate translation quality.Minimum rating is 1, maximum is 10.
        /// service - Rate your experience using OneHourTranslation. Minimum rating is 1, maximum is 5.
        /// </param>
        /// <param name="rate">Rate value, see type for acceptable ranges.</param>
        /// <param name="serviceRate">Extra service / translation rating, values provided via check-boxes.
        /// service
        /// Service rating fields(boolean 0 | 1):
        /// service_was_on_time: 0 - Waited too long | 1 - On time.
        /// service_support_helpful: 0 - Not responsive | 1 - Helpful.
        /// service_good_quality: 0 - Should improve | 1 - Good.
        /// service_trans_responded: 0 - Slowly | 1 - Quickly.
        /// service_would_recommend: 0 - No | 1 - Yes.
        /// </param>
        /// <param name="customerRate">Extra service / translation rating, values provided via check-boxes.
        /// customer
        ///Translation rating check-boxes(boolean 1 | 0 ):
        /// trans_is_good: Good translation quality.
        /// trans_bad_formatting: Bad formatting.
        /// trans_misunderstood_source: Misrepresent / Misunderstood the source.
        /// trans_spell_tps_grmr_mistakes: Spelling / Typos / Grammar mistakes.
        /// trans_text_miss: Missing text / Partly translated.
        /// trans_not_followed_instrctns: Instructions not followed.
        /// trans_inconsistent: Inconsistent translation.
        /// trans_bad_written: Badly written.</param>
        /// <param name="remarks">(optional) You can leave a written review / feedback about the service / translation.</param>
        /// <param name="publish">(optional, default is set to false) Allow OneHourTranslation to post your review / feedback on Yotpo.</param>
        public void RateProject(int projectId, RateType type, int rate, Dictionary<ServiceRate, bool> serviceRate = null, List<CustomerRate> customerRate = null, string remarks = "", bool publish = false)
        {
            var parameters = new List<Param>();            
            parameters.Add(new Param("type", type.GetStringValue()));
            parameters.Add(new Param("rate", rate));

            // flags processed here   
            if(serviceRate != null)
            {
                foreach (var key in serviceRate.Keys)
                {
                    parameters.Add(new Param(string.Format("categories[{0}]", key.GetStringValue()), (serviceRate[key] ? "1" : "0")));
                }
            }                     
            if(customerRate != null)
            {
                foreach (var item in customerRate)
                {
                    parameters.Add(new Param(string.Format("categories[{0}]", item.GetStringValue()), 1));
                }
            }
                       
            // other optional parameters
            if (!string.IsNullOrWhiteSpace(remarks))
                parameters.Add(new Param("remarks", remarks));
            if (publish)
                parameters.Add(new Param("publish", 1));

            // do request
            _request.Post<List<string>>(string.Format("/projects/{0}/rating", projectId), parameters.ToArray());
        }

        /// <summary>
        /// CREATE NEW CONTEXT
        /// POST: /tm/context
        /// </summary>
        /// <param name="name">Context name.</param>
        /// <returns>Context object.</returns>
        public Context CreateContext(string name)
        {
            var result = _request.Post<Context>("/tm/context", new Param("context_name", name));
            return result.Results;
        }

        /// <summary>
        /// CONTEXTS LIST
        /// GET: /tm/context
        /// </summary>
        /// <returns>List of context objects.</returns>
        public List<Context> GetContextsList()
        {
            var result = _request.Get<List<Context>>("/tm/context");
            return result.Results;
        }

        /// <summary>
        /// GET DETAILS OF A SPECIFIC CONTEXT
        /// GET: /tm/context/<context_uuid>
        /// </summary>
        /// <param name="contextUuid">Context UUID.</param>
        /// <returns>Context object.</returns>
        public Context GetContextDetails(string contextUuid)
        {
            var result = _request.Get<Context>(string.Format("/tm/context/{0}", contextUuid));
            return result.Results;
        }

        /// <summary>
        /// DELETE A SPECIFIC CONTEXT AND ITS PHRASES
        /// DELETE: /tm/context/<context_uuid>
        /// </summary>
        /// <param name="contextUuid">Context UUID.</param>
        public void DeleteContext(string contextUuid)
        {
            _request.Delete<List<string>>(string.Format("/tm/context/{0}", contextUuid));
        }

        /// <summary>
        /// CREATE A PHRASE
        /// POST: /tm/context/<context_uuid>/phrases
        /// </summary>
        /// <param name="contextUuid">Context UUID.</param>
        /// <param name="sourceLanguage">Source language code.</param>
        /// <param name="sourceText">Phrase text.</param>
        /// <param name="remarks">Remarks about phrase text.</param>
        /// <returns>Phrase details object.</returns>
        public Phrase CreatePhrase(string contextUuid, string sourceLanguage, string sourceText, string remarks = "")
        {
            var parameters = new List<Param>();
            parameters.Add(new Param("source_language", sourceLanguage));
            parameters.Add(new Param("source_text", sourceText));
            if (!string.IsNullOrWhiteSpace(remarks))
                parameters.Add(new Param("remarks", remarks));
            var result = _request.Post<Phrase>(string.Format("/tm/context/{0}/phrases", contextUuid), parameters.ToArray());
            var phrase = result.Results;
            phrase.Targets = ParsePhaseTargets(phrase.TargetsRaw);
            return phrase;
        }

        /// <summary>
        /// GET A SPECIFIC PHRASE
        /// GET: /tm/context/<context_uuid>/phrase/<phrase_key>
        /// </summary>
        /// <param name="contextUuid">Context UUID.</param>
        /// <param name="phraseKey">Phrase key.</param>
        /// <returns>Phrase details object.</returns>
        public Phrase GetPhrase(string contextUuid, string phraseKey)
        {
            var result = _request.Get<Phrase>(string.Format("/tm/context/{0}/phrase/{1}", contextUuid, phraseKey));
            var phrase = result.Results;
            phrase.Targets = ParsePhaseTargets(phrase.TargetsRaw);
            return phrase;
        }

        /// <summary>
        /// UPDATE SPECIFIC PHRASE
        /// POST: /tm/context/<context_uuid>/phrase/<phrase_key>
        /// </summary>
        /// <param name="contextUuid">Context UUID.</param>
        /// <param name="phraseKey">Phrase key.</param>
        /// <param name="sourceLanguage">Source language code.</param>
        /// <param name="sourceText">Phrase text.</param>
        /// <param name="targetText">Target language code.</param>
        /// <returns>Phrase details object.</returns>
        public Phrase UpdatePhrase(string contextUuid, string phraseKey, string sourceText, string targetLanguage, string targetText)
        {
            var parameters = new List<Param>();
            parameters.Add(new Param("source_text", sourceText));
            parameters.Add(new Param("target_language", targetLanguage));
            parameters.Add(new Param("target_text", targetText));
            var result = _request.Post<Phrase>(string.Format("/tm/context/{0}/phrase/{1}", contextUuid, phraseKey), parameters.ToArray());
            var phrase = result.Results;
            phrase.Targets = ParsePhaseTargets(phrase.TargetsRaw);
            return phrase;
        }

        /// <summary>
        /// DELETE SPECIFIC PHRASE
        /// DELETE: /tm/context/<context_uuid>/phrase/<phrase_key>
        /// </summary>
        /// <param name="contextUuid">Context UUID.</param>
        /// <param name="phraseKey">Phrase key.</param>
        public void DeletePhrase(string contextUuid, string phraseKey)
        {
            _request.Delete<List<string>>(string.Format("/tm/context/{0}/phrase/{1}", contextUuid, phraseKey));
        }

        /// <summary>
        /// CREATE A NEW PROJECT
        /// This request acts the same as “Project > Create a New Project” request, but instead of manually specifying source-resources, all untranslated strings in the context are automatically wrapped as a source-resource.
        /// </summary>
        /// <param name="contextUuid">Context UUID.</param>
        /// <returns>Project details object.</returns>
        public Project CreateProject(string contextUuid)
        {
            var result = _request.Post<Project>(string.Format("/tm/context/{0}/translate", contextUuid));
            return result.Results;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Parses dynamic targets object into list of PhraseTarget objects
        /// </summary>
        /// <param name="targets">Text source of targets object.</param>
        /// <returns>List of PhraseTarget objects</returns>
        List<PhraseTarget> ParsePhaseTargets(string targets)
        {
            var result = new List<PhraseTarget>();
            var obj = JsonConvert.DeserializeObject<JObject>(targets);
            JToken token = obj.First;
            while (token != null)
            {
                var target = new PhraseTarget();
                target.TargetLanguage = token.Path;
                target.TargetPhrases = new List<string>();
                if (token.First != null)
                {
                    var word = token.First.First;
                    while (word != null)
                    {
                        target.TargetPhrases.Add(word.Value<string>());
                        word = word.Next;
                    }
                }

                result.Add(target);
                token = token.Next;
            }

            return result;
        }

        /// <summary>
        /// Parsing project's resource binding data structure
        /// </summary>
        /// <param name="project">Project object</param>
        private void ParseProjectResourceBinding(ProjectDetails project)
        {
            if (project.ResourceBindingRaw == null)
                return;
            project.ResourceBinding = new List<ProjectResourceBinding>();

            JObject bindingsRaw = project.ResourceBindingRaw;
            var bindingElem = bindingsRaw.First;
            while (bindingElem != null)
            {
                if (!string.IsNullOrWhiteSpace(bindingElem.Path))
                {
                    ProjectResourceBinding binding = new Entities.ProjectResourceBinding();
                    binding.FromResource = bindingElem.Path;
                    // If array of bind to resources is not empty 
                    if (bindingElem.First != null && bindingElem.First.HasValues)
                    {
                        var bindTo = bindingElem.First.First;
                        if (bindTo != null)
                        {
                            binding.ToResources = new List<string>();
                            while (bindTo != null)
                            {
                                binding.ToResources.Add(bindTo.Value<string>());
                                bindTo = bindTo.Next;
                            }
                        }
                    }

                    project.ResourceBinding.Add(binding);
                }
                bindingElem = bindingElem.Next;
            }
        }

        /// <summary>
        /// Common private method to create different types of projects
        /// </summary>
        /// <param name="url">Relative url of the appropriate project method.</param>
        /// <param name="sourceLanguage">Source language code.</param>
        /// <param name="targetLanguage">Target language code.</param>
        /// <param name="sources">List of Resource UUIDs. You can create a project with up to 30 resources at a time. For more than 30 resources please create a new project.</param>
        /// <param name="refResources">(optional) List of reference resource UUIDs.</param>
        /// <param name="wordCount">(optional) Integer, if empty use automatic counting.</param>
        /// <param name="notes">(optional) Text note that will be shown to translator regarding the new project.</param>
        /// <param name="expertise">(optional) Expertise code.</param>
        /// <param name="name">(optional) Name your project. If empty, your project will be named automatically.</param>
        /// <param name="callbackUrl">(optional) Callback URL.</param>
        /// <param name="custom">(optional) Custom callback parameters [0...9]</param>
        /// <returns>Project details object</returns>
		private Project CreateProject(string url, string sourceLanguage, string targetLanguage = "", List<string> sources = null, List<string> translations = null, List<string> refResources = null,
            int wordCount = 0, string notes = "", ExpertiseType expertise = ExpertiseType.None, string name = "", string callbackUrl = "", params string[] custom)
        {
            var parameters = new List<Param>();
            parameters.Add(new Param("source_language", sourceLanguage));
            if (!string.IsNullOrWhiteSpace(targetLanguage))
                parameters.Add(new Param("target_language", targetLanguage));
            if (sources != null)
                parameters.Add(new Param("sources", string.Join<string>(",", sources)));
            if (translations != null)
                parameters.Add(new Param("translations", string.Join<string>(",", translations)));
            if (refResources != null && refResources.Count > 0)
                parameters.Add(new Param("reference_resources", string.Join<string>(",", refResources)));
            if (wordCount > 0)
                parameters.Add(new Param("wordcount", wordCount));
            if (!string.IsNullOrWhiteSpace(notes))
                parameters.Add(new Param("notes", notes));
            if (expertise != ExpertiseType.None)
                parameters.Add(new Param("expertise", expertise.GetStringValue()));
            if (!string.IsNullOrWhiteSpace(name))
                parameters.Add(new Param("name", name));
            if (!string.IsNullOrWhiteSpace(callbackUrl))
                parameters.Add(new Param("callback_url", callbackUrl));
            if (custom != null && custom.Length > 0)
            {
                for (int i = 0; i < custom.Length && i < 10; i++)
                {
                    if (!string.IsNullOrWhiteSpace(custom[i]))
                    {
                        parameters.Add(new Param("custom" + i.ToString(), custom[i]));
                    }
                }
            }

            var result = _request.Post<Project>(url, parameters.ToArray());
            return result.Results;
        }

        #endregion
    }
}
