using System;
using System.Net.Http;

namespace Wikiled.Common.Net.Client
{
    public class RequestException : Exception
    {
        public RequestException(HttpResponseMessage response, string responceText, Exception innerException)
        {
            Response = response;
            ResponceText = responceText;
            InnerException = innerException;
        }

        public string ResponceText { get; }

        public new Exception InnerException { get; }

        public HttpResponseMessage Response { get; }
    }
}
