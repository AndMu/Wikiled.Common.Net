using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace Wikiled.Common.Net.Client
{
    public class StreamApiClientFactory : IStreamApiClientFactory
    {
        private readonly ILogger<StreamApiClient> logger;

        private readonly Uri url;

        private readonly HttpClient client;

        public StreamApiClientFactory(ILogger<StreamApiClient> logger, HttpClient client, Uri url)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.url = url ?? throw new ArgumentNullException(nameof(url));
        }

        public StreamApiClientFactory(ILogger<StreamApiClient> logger, Uri url)
            : this(logger, new HttpClient { Timeout = TimeSpan.FromMinutes(20) }, url)
        {
        }

        public IStreamApiClient Contruct()
        {
            return new StreamApiClient(client, url, logger);
        }
    }
}
