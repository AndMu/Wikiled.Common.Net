using System;
using System.Net.Http;

namespace Wikiled.Common.Net.Client
{
    public class ServiceException : Exception
    {
        public ServiceException(HttpResponseMessage response, string responseText)
        {
            Response = response;
            ResponseText = responseText;
        }

        public string ResponseText { get; }

        public HttpResponseMessage Response { get; }
    }
}
