using System.Net;

namespace Wikiled.Common.Net.Resilience;

public class ResilienceConfig : IResilienceConfig
{
    public HttpStatusCode[] RetryCodes { get; set; }

    public HttpStatusCode[] LongRetryCodes { get; set; }

    public int LongDelay { get; set; }

    public int ShortDelay { get; set; }

    public static IResilienceConfig GenerateCommon()
    {
        return new ResilienceConfig
        {
            LongDelay = 1000 * 1000,
            ShortDelay = 1000,
            LongRetryCodes = new[] { HttpStatusCode.Forbidden },
            RetryCodes = new[]
            {
                HttpStatusCode.RequestTimeout,
                HttpStatusCode.InternalServerError,
                HttpStatusCode.BadGateway,
                HttpStatusCode.ServiceUnavailable,
                HttpStatusCode.GatewayTimeout
            }
        };
    }
}