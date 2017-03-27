using System;
using System.Text;
using System.Linq;
using System.Net;
using System.Web;
using System.Collections.Generic;

using Newtonsoft.Json;

using RestSharp;
using RestSharp.Extensions;

namespace oht.Restapi
{
    /// <summary>
    /// Request class
    /// </summary>
    internal sealed class Request
    {
        #region internal members
        private string _publicKey;
        private string _secretKey;        
        private RestClient _client;                
        #endregion

        /// <summary>
        /// Custom public constructor
        /// </summary>
        /// <param name="publicKey">Public key</param>
        /// <param name="secretKey">Secret key</param>
        /// <param name="baseUrl">Base API url</param>
        /// <param name="proxy">HTTP proxy instance</param>
        public Request(string publicKey, string secretKey, string baseUrl, WebProxy proxy)
        {
            _publicKey = publicKey;
            _secretKey = secretKey;            
            _client = new RestClient(baseUrl);
            if(proxy != null)
                _client.Proxy = proxy;
        }
        
        /// <summary>
        /// Default constructor is hidden
        /// </summary>
        private Request()
        {
        }        

        /// <summary>
        /// HTTP GET method implementation
        /// </summary>
        /// <typeparam name="ResultType">Returned entity type</typeparam>
        /// <param name="url">Method URL</param>
        /// <param name="parameters">Additional method parameters</param>
        /// <returns>Generic, expected result enveloped with standard response object</returns>
        public Response<ResultType> Get<ResultType>(string url, params Param[] parameters) where ResultType : new()
        {                              
            return DoRequest<ResultType>(url, Method.GET, parameters);
        }

        /// <summary>
        /// HTTP POST method implementation
        /// </summary>
        /// <typeparam name="ResultType">Returned entity type</typeparam>
        /// <param name="url">Method URL</param>
        /// <param name="parameters">Additional method parameters</param>
        /// <returns>Generic, expected result enveloped with standard response object</returns>
        public Response<ResultType> Post<ResultType>(string url, params Param[] parameters) where ResultType : new()
        {
            return DoRequest<ResultType>(url, Method.POST, parameters);
        }


        /// <summary>
        /// HTTP DELETE method implementation
        /// </summary>
        /// <typeparam name="ResultType">Returned entity type</typeparam>
        /// <param name="url">Method URL</param>
        /// <param name="parameters">Additional method parameters</param>
        /// <returns>Generic, expected result enveloped with standard response object</returns>
        public Response<ResultType> Delete<ResultType>(string url, params Param[] parameters) where ResultType : new()
        {
            return DoRequest<ResultType>(url, Method.DELETE, parameters);
        }

        #region private methods
        /// <summary>
        /// Main method to perform any type of request the the REST API
        /// </summary>
        /// <typeparam name="ResultType">Returned entity type</typeparam>
        /// <param name="url">Method URL</param>
        /// <param name="fileContent">Content of the file to be uploaded, optional</param>
        /// <param name="method">HTTP nethod, e.g. GET, POST</param>
        /// <param name="parameters">Additional method parameters</param>
        /// <returns></returns>
        private Response<ResultType> DoRequest<ResultType>(string url, Method method, params Param[] parameters) where ResultType : new()
        {
            try
            {
                // Create request instance
                var resourceUrl = GetResourceUrl(url, method, parameters);                
                var request = new RestRequest(resourceUrl, method);

                // If download request - no data returned, file is saved straight away
                if(method == Method.GET && parameters != null && parameters.Any(p => p.Type == ParamType.FilePath))
                {
                    var param = parameters.SingleOrDefault(p => p.Type == ParamType.FilePath);
                    _client.DownloadData(request).SaveAs(param.Value.ToString());
                    return null;
                }

                // If file path provided - adding it to the request body
                if(method == Method.POST && parameters != null && parameters.Any(p => p.Type == ParamType.FilePath))
                {
                    var param = parameters.SingleOrDefault(p => p.Type == ParamType.FilePath);
                    string contentType = GetParamValue("file_mime", parameters);
                    request.AddFile(param.Name, param.Value.ToString(), contentType);
                }

                // If file content provided - adding it to the request body
                if (method == Method.POST && parameters != null && parameters.Any(p => p.Type == ParamType.FileContent))
                {
                    var param = parameters.SingleOrDefault(p => p.Type == ParamType.FileContent);
                    string fileName = GetParamValue("file_name", parameters);
                    string contentType = GetParamValue("file_mime", parameters);                    
                    string content = param.Value.ToString();
                    request.AddFileBytes(param.Name, Encoding.Default.GetBytes(content), fileName, contentType);                    
                }

                // All other parameters goes straight to Request if POST or PUT method
                if((method == Method.POST || method == Method.PUT) && parameters.Any(p => p.Type == ParamType.Object ))
                {
                    foreach (var param in parameters.Where(p => p.Type == ParamType.Object))
                    {
                        request.AddParameter(param.Name, param.Value);
                    }
                }
                
                // Do request
                var response = _client.Execute(request);                
                if (response == null)
                    throw new OhtException("OneHourTranslation response was malformed.");
                if(response.StatusCode != HttpStatusCode.OK)
                    throw new OhtException(string.Format("OneHourTranslation HTTP request failed with response code {0}({1}) and message '{2}'", response.StatusCode, response.StatusDescription, response.ErrorMessage), response.ErrorException);

                // Pulls expected data. Do trycatch - if fails, will try to parse different way - with results as array of string.
                Response<ResultType> data = null;
                Exception primaryException = null;
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                
                try
                {                    
                    data = JsonConvert.DeserializeObject<Response<ResultType>>(response.Content, settings);
                }
                catch(Exception ex)
                {
                    primaryException = ex;
                }                

                // Some content is not compatible with expected return type - most likely error
                if (data == null)
                {
                    Response<List<string>> localData = null;
                    try
                    {
                        localData = JsonConvert.DeserializeObject<Response<List<string>>>(response.Content, settings);
                    }
                    catch(Exception ex)
                    {
                        var exception = new OhtException("Unexpected error occured for data deserialization", ex);
                        exception.PrimaryException = primaryException;
                        throw exception;
                    }
                    

                    if (localData != null && localData.Status != null && localData.Status.Code > 0)
                    {
                        var exception = new OhtException(localData.Status.Code, localData.Status.Message, localData.Errors);
                        exception.PrimaryException = primaryException;
                        throw exception;
                    }
                    else if(!(localData != null && localData.Status != null && localData.Status.Code == 0 && localData.Status.Message == "ok"))
                    {
                        var exception = new OhtException("OneHourTranslation returned unexpected API response: " + response.Content);
                        exception.PrimaryException = primaryException;
                        throw exception;                        
                    }

                    //This is for the case when empty results object is returned
                    data = new Response<ResultType>();
                    data.Status = localData.Status;
                    data.Results = default(ResultType);                                                            
                }

                // If no Status object - some odd response received
                if(data.Status == null)
                    throw new OhtException("OneHourTranslation returned unexpected API response: " + response.Content);

                // If expected data deserialized correctly
                if (data.Status.Code == 0 && data.Status.Message.ToLower() == "ok")
                {
                    return data;
                }
                else
                {
                    throw new OhtException(data.Status.Code, data.Status.Message, data.Errors);
                }                                                                
            }
            catch(OhtException ex)
            {
                throw ex;
            }
            catch(Exception ex)
            {
                throw new OhtException("Unknown API error occured doing request to OneHourTranslation", ex);
            }            
        }

        /// <summary>
        /// Pulls specific parameter value from the list of additional parameters of the request
        /// </summary>
        /// <param name="parameters">List of additional parameters</param>
        /// <returns>string</returns>
        private string GetParamValue(string name, params Param[] parameters)
        {
            string result = null;
            if (parameters.Any(p => p.Name.ToLower() == name))
            {
                var mimeParam = parameters.SingleOrDefault(p => p.Name.ToLower() == name);
                if (mimeParam != null)
                    result = mimeParam.Value.ToString();
            }
            return result;
        }        

        /// <summary>
        /// Forms resource url string based on keys we have and extra parameters passed
        /// </summary>
        /// <param name="url">Method url</param>
        /// <param name="parameters">Method parameters</param>
        /// <returns></returns>
        private string GetResourceUrl(string url, Method method, params Param[] parameters)
        {

            string result = string.Format("{0}?public_key={1}", EnsureLeadingSlash(url), _publicKey);
            if(!string.IsNullOrWhiteSpace(_secretKey))
                result += string.Format("&secret_key={0}", _secretKey);

            if(method == Method.GET || method == Method.DELETE)
            {
                foreach (var param in parameters.Where(p => p.Type == ParamType.Object))
                {
                    result += string.Format("&{0}={1}", param.Name, HttpUtility.UrlEncode(param.Value.ToString()));
                }
            }
            
            return result;
        }

        /// <summary>
        /// Adds leading slash to the relative url if required
        /// </summary>
        /// <param name="url">Relative url to look at</param>
        /// <returns></returns>
        private string EnsureLeadingSlash(string url)
        {
            var result = url;
            if (!result.StartsWith("/"))
                result = "/" + result;
            return result;
        }
        #endregion
    }
}
