using System.Threading.Tasks;

namespace Wikiled.Common.Net.Client
{
    public class ResponseExtension
    {
        public async Task<T> ProcessResult<T>(ServiceResponse<RawResponse<T>> response)
        {
            if (!response.IsSuccess)
            {
                var content = await response.HttpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new ServiceException(response.HttpResponseMessage, content);
            }

            return response.Result.Value;
        }

        public async Task<T> ProcessResult<T>(ServiceResponse<ServiceResult<T>> response)
        {
            if (!response.IsSuccess)
            {
                var content = await response.HttpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new ServiceException(response.HttpResponseMessage, content);
            }

            return response.Result.Value;
        }
    }
}
