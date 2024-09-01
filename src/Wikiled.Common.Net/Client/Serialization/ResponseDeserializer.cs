using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Wikiled.Common.Net.Client.Serialization;

public class ResponseDeserializer : IResponseDeserializer
{
    public async Task<ServiceResponse<TResult>> GetData<TResult>(HttpResponseMessage response)
        where TResult : IApiResponse
    {
        string responseBody = default;
        try
        {
            responseBody = response.Content == null
                ? null
                : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            TResult data = default;
            if (!string.IsNullOrEmpty(responseBody) &&
                response.IsSuccessStatusCode)
            {
                data = JsonSerializer.Deserialize<TResult>(responseBody, ProtocolSettings.SerializerOptions);
            }

            return ServiceResponse<TResult>.CreateResponse(response, responseBody, data);
        }
        catch (Exception e)
        {
            throw new ServiceException(response, responseBody, e);
        }
    }
}