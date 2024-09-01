using System.Threading;
using System.Threading.Tasks;
using Wikiled.Common.Net.Resilience;

namespace Wikiled.Common.Net.Client;

public class ResilientApiClient : IResilientApiClient
{
    private readonly IApiClient client;

    private readonly IResilience resilience;

    public ResilientApiClient(IApiClient client, IResilience resilience)
    {
        this.client = client;
        this.resilience = resilience;
    }

    public Task<T> GetRequest<T>(string action, CancellationToken token)
    {
        return resilience.WebPolicy.ExecuteAsync(() => client.GetRequest<RawResponse<T>>(action, token).ProcessResult());
    }

    public Task<TResult> GetRequest<TResult, TInput>(string action, TInput data, CancellationToken token)
    {
        return resilience.WebPolicy.ExecuteAsync(
            () => client.PostRequest<TInput, RawResponse<TResult>>(action, data, token).ProcessResult());
    }
}