using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using Wikiled.Common.Net.Client.Serialization;
using Wikiled.Common.Net.Resilience;

namespace Wikiled.Common.Net.Client;

public sealed class GenericClientFactory : IGenericClientFactory
{
    private readonly ILoggerFactory logger;

    private readonly HttpClient client;

    private readonly IResilience resilience;

    public GenericClientFactory(ILoggerFactory logger, HttpClient client, IResilience resilience)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.resilience = resilience ?? throw new ArgumentNullException(nameof(resilience));
    }

    public IStreamApiClient ConstructStreaming(Uri url)
    {
        return new StreamApiClient(client, url, logger);
    }

    public IApiClient ConstructRegular(Uri url)
    {
        return new ApiClient(client, url, new ResponseDeserializerFactory());
    }

    public IResilientApiClient ConstructResilient(Uri url)
    {
        return new ResilientApiClient(new ApiClient(client, url, new ResponseDeserializerFactory()), resilience);
    }
}