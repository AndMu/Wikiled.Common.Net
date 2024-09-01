using System.Net.Http;
using System.Text.Json;

namespace Wikiled.Common.Net.Client;

public class ServiceResponse<T> : IServiceResponse
    where T : IApiResponse
{
    private ServiceResponse()
    {
    }

    public HttpResponseMessage HttpResponseMessage { get; private set; }

    public T Result { get; private set; }

    public string Body { get; private set; }

    public bool IsSuccess => HttpResponseMessage.IsSuccessStatusCode;

    public static ServiceResponse<T> CreateResponse(HttpResponseMessage message, string body, T data)
    {
        var instance = new ServiceResponse<T>();
        instance.Body = body;
        instance.Result = data;
        instance.HttpResponseMessage = message;
        return instance;
    }

    public TResult ReadAs<TResult>()
    {
        return JsonSerializer.Deserialize<TResult>(Body, ProtocolSettings.SerializerOptions);
    }
}