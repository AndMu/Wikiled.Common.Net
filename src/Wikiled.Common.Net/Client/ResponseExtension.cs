using System.Threading.Tasks;

namespace Wikiled.Common.Net.Client
{
    public static class ResponseExtension
    {
        public static async Task<T> ProcessResult<T>(this Task<ServiceResponse<RawResponse<T>>> responseTask)
        {
            var response = await responseTask.ConfigureAwait(false);
            if (!response.IsSuccess)
            {
                var content = await response.HttpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new ServiceException(response.HttpResponseMessage, content);
            }

            return response.Result.Value;
        }

        public static async Task<T> ProcessResult<T>(this Task<ServiceResponse<ServiceResult<T>>> responseTask)
        {
            var response = await responseTask.ConfigureAwait(false);
            if (!response.IsSuccess)
            {
                var content = await response.HttpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new ServiceException(response.HttpResponseMessage, content);
            }

            return response.Result.Value;
        }
    }
}
