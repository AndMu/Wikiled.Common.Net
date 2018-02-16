using Newtonsoft.Json;

namespace Wikiled.Common.Net.Client
{
    public class ServiceResult<T> : IApiResponse
    {
        public ServiceResult(int statusCode, T value, string message)
        {
            StatusCode = statusCode;
            Value = value;
            Message = message;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Value { get;  }

        public int StatusCode { get; }

        public string Message { get; }
    }
}
