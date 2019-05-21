using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Wikiled.Common.Net.Client;

namespace Wikiled.Common.Net.Resilience
{
    public class CommonResilience : IResilience
    {
        private readonly ILogger<CommonResilience> logger;

        private readonly IResilienceConfig config;

        public CommonResilience(ILogger<CommonResilience> logger, IResilienceConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (config.LongRetryCodes == null)
            {
                throw new ArgumentNullException(nameof(config.LongRetryCodes));
            }

            if (config.LongDelay <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(config.LongDelay));
            }

            if (config.ShortDelay <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(config.ShortDelay));
            }

            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.config = config;
            var httpStatusCodesWorthRetrying = config.LongRetryCodes.Concat(config.RetryCodes).ToArray();

            WebPolicy = Policy
                .Handle(WebExceptionPredicate(httpStatusCodesWorthRetrying))
                .Or(RequestExceptionPredicate(httpStatusCodesWorthRetrying))
                .Or<IOException>()
                .WaitAndRetryAsync(5,
                                   (retries, ex, ctx) => DelayRoutine(ex, retries),
                                   (ts, i, ctx, task) => Task.CompletedTask);
        }

        public IAsyncPolicy WebPolicy { get; }

        private TimeSpan DelayRoutine(Exception ex, int retries)
        {
            var webException = ex as WebException;
            if (webException == null)
            {
                var waitTime = TimeSpan.FromMilliseconds(retries * config.ShortDelay);
                logger.LogError(ex, "Error detected. Waiting {0}", waitTime);
                return waitTime;
            }

            var response = webException.Response as HttpWebResponse;
            var errorCode = response?.StatusCode;
            if (errorCode == null ||
                !config.LongRetryCodes.Contains(errorCode.Value))
            {
                var waitTime = TimeSpan.FromMilliseconds(retries * config.ShortDelay);
                logger.LogError(ex, "Web Error detected. Waiting {0}", waitTime);
                return waitTime;
            }

            var wait = TimeSpan.FromMilliseconds(config.LongDelay);
            logger.LogError(ex, "Forbidden detected. Waiting {0}", wait);
            return wait;
        }

        private Func<RequestException, bool> RequestExceptionPredicate(HttpStatusCode[] httpStatusCodes)
        {
            return exception =>
            {
                if (exception.InnerException is WebException webException)
                {
                    return WebExceptionPredicate(httpStatusCodes)(webException);
                }

                return true;
            };
        }

        private Func<WebException, bool> WebExceptionPredicate(HttpStatusCode[] httpStatusCodes)
        {
            return exception => !(exception.Response is HttpWebResponse response) || httpStatusCodes.Contains(response.StatusCode);
        }
    }
}
