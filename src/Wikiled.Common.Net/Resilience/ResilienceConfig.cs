using System.Net;

namespace Wikiled.Common.Net.Resilience
{
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
                    HttpStatusCode.RequestTimeout,      // 408
                    HttpStatusCode.InternalServerError, // 500
                    HttpStatusCode.BadGateway,          // 502
                    HttpStatusCode.ServiceUnavailable,  // 503
                    HttpStatusCode.GatewayTimeout       // 504
                }
            };
        }
    }
}
