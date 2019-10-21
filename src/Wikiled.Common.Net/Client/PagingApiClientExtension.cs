using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wikiled.Common.Net.Paging;

namespace Wikiled.Common.Net.Client
{
    public static class PagingApiClientExtension
    {
        public static async Task<PagedList<TResult>> PostPagingRequest<TResult>(this IApiClient client, string path, PagingInfo info, CancellationToken token)
        {
            var result = await client.PostRequest<PagingInfo, RawResponse<TResult[]>>(path, info, token)
                                      .ConfigureAwait(false);

            return ProcessResult(info, result);
        }

        private static PagedList<TResult> ProcessResult<TResult>(PagingInfo info, ServiceResponse<RawResponse<TResult[]>> result)
        {
            if (!result.IsSuccess)
            {
                throw new ApplicationException("Failed to retrieve:" + result.HttpResponseMessage);
            }

            var resultItems = Array.Empty<TResult>();
            if (result.Result?.Value != null)
            {
                resultItems = result.Result.Value;
            }

            long count = 0;
            if (result.HttpResponseMessage.Headers.TryGetValues(PagingConstants.TotalHeader, out var header))
            {
                var item = header.FirstOrDefault();
                if (item != null)
                {
                    _ = long.TryParse(item, out count);
                }
            }

            return new PagedList<TResult>(resultItems, count, info);
        }
    }
}
