using System.Threading;
using System.Threading.Tasks;
using Wikiled.Common.Net.Paging;

namespace Wikiled.Common.Net.Client
{
    public interface IApiClient
    {
        Task<ServiceResponse<TResult>> PostRequest<TInput, TResult>(string action, TInput argument, CancellationToken token)
            where TResult : IApiResponse;

        Task<ServiceResponse<TResult>> GetRequest<TResult>(string action, CancellationToken token)
            where TResult : IApiResponse;
    }
}