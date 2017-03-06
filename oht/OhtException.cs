using System;
using System.Collections.Generic;

namespace oht
{
    /// <summary>
    /// API custom exception used to cover various errors API may raise
    /// </summary>
    public class OhtException : Exception
    {       
        public OhtException() : base() { }
        public OhtException(string message) : base(message) { }
        public OhtException(string message, Exception innerException) : base(message, innerException) { }

        public OhtException(int statusCode, string statusMessage, List<string> errors) : this(statusCode, statusMessage, errors, null) { }

        public OhtException(int statusCode, string statusMessage, List<string> errors, Exception innerException) : base(statusMessage, innerException)
        {
            StatusCode = statusCode;
            Errors = errors;
        }

        /// <summary>
        /// Status code returned by OneHourTransalte API
        /// </summary>
        public int StatusCode { get; private set; }

        /// <summary>
        /// Primary error message returned by OneHourTransalte API
        /// </summary>
        public string StatusMessage { get { return this.Message; } }

        /// <summary>
        /// Additional error messages returned by OneHourTransalte API
        /// </summary>
        public List<string> Errors { get; private set; }

        /// <summary>
        /// Additional exception object which may occur before this current one and makes sense to be considered as well
        /// </summary>
        public Exception PrimaryException { get; set; }
    }
}
