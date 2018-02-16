using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Wikiled.Common.Net.Client.Serialization
{
    public class ResponseDeserializer : IResponseDeserializer
    {

        public async Task<ServiceResponse<TResult>> GetData<TResult>(HttpResponseMessage response)
            where TResult : IApiResponse
        {
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            TResult data = default(TResult);
            if (!string.IsNullOrEmpty(responseBody))
            {
                data = JsonConvert.DeserializeObject<TResult>(responseBody);
            }

            return ServiceResponse<TResult>.CreateResponse(response, responseBody, data);
        }
    }
}
