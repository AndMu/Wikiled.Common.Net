using System;
using System.Net.Http;
using Wikiled.Common.Net.Client.Serialization;

namespace Wikiled.Common.Net.Client;

public class ApiClientFactory : IApiClientFactory
{
    private readonly HttpClient client;

    private readonly Uri baseUri;

    public ApiClientFactory(HttpClient client, Uri baseUri)
    {
        this.baseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public IApiClient GetClient()
    {
        return new ApiClient(client, baseUri, new ResponseDeserializerFactory());
    }
}