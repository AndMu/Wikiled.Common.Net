using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using Wikiled.Common.Net.Client.Serialization;

namespace Wikiled.Common.Net.Client
{
    public sealed class GenericClientFactory : IGenericClientFactory
    {
        private readonly ILoggerFactory logger;

        private readonly HttpClient client;

        public GenericClientFactory(ILoggerFactory logger, HttpClient client)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public IStreamApiClient ConstructStreaming(Uri url)
        {
            return new StreamApiClient(client, url, logger);
        }

        public IApiClient ConstructRegular(Uri url)
        {
            return new ApiClient(client, url, new ResponseDeserializerFactory());
        }
    }
}
