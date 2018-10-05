using System.Net.Http;
using Newtonsoft.Json;

namespace Wikiled.Common.Net.Client
{
    public class ServiceResponse<T> 
        where T : IApiResponse
    {
        private ServiceResponse()
        {
        }

        public HttpResponseMessage HttpResponseMessage { get; private set; }

        public T Result { get; private set; }

        public string Body { get; private set; }

        public bool IsSuccess => HttpResponseMessage.IsSuccessStatusCode;

        public TResult ReadAs<TResult>()
        {
            return JsonConvert.DeserializeObject<TResult>(Body);
        }

        public static ServiceResponse<T> CreateResponse(HttpResponseMessage message, string body, T data)
        {
            var instance = new ServiceResponse<T>();
            instance.Body = body;
            instance.Result = data;
            instance.HttpResponseMessage = message;
            return instance;
        }
    }
}
