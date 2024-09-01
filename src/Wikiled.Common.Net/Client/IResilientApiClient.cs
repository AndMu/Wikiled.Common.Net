using System.Threading;
using System.Threading.Tasks;

namespace Wikiled.Common.Net.Client;

public interface IResilientApiClient
{
    Task<T> GetRequest<T>(string action, CancellationToken token);

    Task<TResult> GetRequest<TResult, TInput>(string action, TInput data, CancellationToken token);
}