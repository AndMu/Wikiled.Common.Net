using Polly;

namespace Wikiled.Common.Net.Resilience;

public interface IResilience
{
    IAsyncPolicy WebPolicy { get; }
}