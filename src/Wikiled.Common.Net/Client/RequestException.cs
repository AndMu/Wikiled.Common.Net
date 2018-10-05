﻿using System;
using System.Net.Http;

namespace Wikiled.Common.Net.Client
{
    public class RequestException : Exception
    {
        public RequestException(HttpResponseMessage response, string responseText, Exception innerException)
        {
            Response = response;
            ResponseText = responseText;
            InnerException = innerException;
        }

        public string ResponseText { get; }

        public new Exception InnerException { get; }

        public HttpResponseMessage Response { get; }
    }
}