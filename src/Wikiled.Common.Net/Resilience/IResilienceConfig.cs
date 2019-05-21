using System.Net;

namespace Wikiled.Common.Net.Resilience
{
    public interface IResilienceConfig
    {
        HttpStatusCode[] RetryCodes { get; }

        HttpStatusCode[] LongRetryCodes { get; }

        int LongRetryDelay { get; }

        int ShortRetryDelay { get; }
    }
}
