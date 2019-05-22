using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace Wikiled.Common.Net.Client
{
    public class StreamApiClientFactory : IStreamApiClientFactory
    {
        private readonly ILoggerFactory logger;

        private readonly Uri url;

        private readonly HttpClient client;

        public StreamApiClientFactory(ILoggerFactory logger, HttpClient client, Uri url)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.url = url ?? throw new ArgumentNullException(nameof(url));
        }

        public StreamApiClientFactory(ILoggerFactory logger, Uri url)
            : this(logger, new HttpClient { Timeout = TimeSpan.FromMinutes(20) }, url)
        {
        }

        public IStreamApiClient Construct()
        {
            return new StreamApiClient(client, url, logger);
        }
    }
}
