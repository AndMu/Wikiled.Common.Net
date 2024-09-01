using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Wikiled.Common.Net.Client;

namespace Wikiled.Common.Net.Resilience;

public class CommonResilience : IResilience
{
    private readonly ILogger<CommonResilience> logger;

    private readonly IResilienceConfig config;

    private readonly ILookup<HttpStatusCode, HttpStatusCode> httpStatusCodesWorthRetrying;

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
        httpStatusCodesWorthRetrying =
            config.LongRetryCodes.Concat(config.RetryCodes).ToLookup(item => item, item => item);

        WebPolicy = Policy
            .Handle(WebException())
            .Or(ServiceException())
            .Or<IOException>()
            .WaitAndRetryAsync(
                5,
                (retries, ex, ctx) => DelayRoutine(ex, retries),
                (ts, i, ctx, task) => Task.CompletedTask);
    }

    public IAsyncPolicy WebPolicy { get; }

    private TimeSpan DelayRoutine(Exception ex, int retries)
    {
        var errorCode = GetErrorCode(ex);
        var waitTime = TimeSpan.FromMilliseconds(retries * config.ShortDelay);

        if (errorCode == null)
        {
            logger.LogError(ex, "Generic Error detected ({1}). Waiting {0}...", waitTime, ex.Message);
            return waitTime;
        }

        if (config.LongRetryCodes.Contains(errorCode.Value))
        {
            waitTime = TimeSpan.FromMilliseconds(config.LongDelay);
            logger.LogError(ex, "Long delay detected ({1}). Waiting {0}...", waitTime, errorCode);
            return waitTime;
        }

        logger.LogError(ex, "Web Error ({1}) detected. Waiting {0}...", waitTime, errorCode);
        return waitTime;
    }

    private HttpStatusCode? GetErrorCode(Exception ex)
    {
        switch (ex)
        {
            case ServiceException serviceException:
                return serviceException.Response.StatusCode;
            case WebException webException:
            {
                var response = webException.Response as HttpWebResponse;
                return response?.StatusCode;
            }

            default:
                return null;
        }
    }

    private Func<ServiceException, bool> ServiceException()
    {
        return exception => httpStatusCodesWorthRetrying.Contains(exception.Response.StatusCode);
    }

    private Func<WebException, bool> WebException()
    {
        return exception =>
        {
            if (exception.Response is HttpWebResponse response)
            {
                return httpStatusCodesWorthRetrying.Contains(response.StatusCode);
            }

            return true;
        };
    }
}