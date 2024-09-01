using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Wikiled.Common.Net.Client.Serialization;

namespace Wikiled.Common.Net.Client;

public class ApiClient : IApiClient
{
    private readonly Uri baseUri;

    private readonly HttpClient client;

    private readonly IResponseDeserializerFactory deserializer;

    public ApiClient(HttpClient client, Uri baseUri, IResponseDeserializerFactory deserializer)
    {
        this.deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        this.baseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
    }
   
    public async Task<ServiceResponse<TResult>> PostRequest<TInput, TResult>(string action, TInput argument, CancellationToken token) 
        where TResult : IApiResponse
    {
        var response = await client.PostAsync(new Uri(baseUri, action), new StringContent(JsonSerializer.Serialize(argument, ProtocolSettings.SerializerOptions), Encoding.UTF8, "application/json"), token).ConfigureAwait(false);
        return await deserializer.Construct<TResult>().GetData<TResult>(response).ConfigureAwait(false);
    }

    public async Task<ServiceResponse<TResult>> GetRequest<TResult>(string action, CancellationToken token) 
        where TResult : IApiResponse
    {
        var response = await client.GetAsync(new Uri(baseUri, action), token).ConfigureAwait(false);
        return await deserializer.Construct<TResult>().GetData<TResult>(response).ConfigureAwait(false);
    }
}