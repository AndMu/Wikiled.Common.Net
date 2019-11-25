using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Wikiled.Common.Reflection;

namespace Wikiled.Common.Net.Client.Serialization
{
    public class RawResponseDeserializer : IResponseDeserializer
    {
        private static readonly ConcurrentDictionary<Type, Type> resolutionCache = new ConcurrentDictionary<Type, Type>();

        public async Task<ServiceResponse<TResult>> GetData<TResult>(HttpResponseMessage response)
            where TResult : IApiResponse
        {
            string responseBody = default;
            try
            {
                responseBody = response.Content == null
                    ? null
                    : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                TResult result = default;
                var type = typeof(TResult);

                if (!string.IsNullOrEmpty(responseBody) && 
                    response.IsSuccessStatusCode)
                {
                    if (!resolutionCache.TryGetValue(type, out var resultType))
                    {
                        var genericTypes = type.GenericTypeArguments;
                        if (genericTypes.Length != 1)
                        {
                            throw new ArgumentOutOfRangeException(nameof(TResult), "RawResponse<T> is supported");
                        }

                        resultType = type.GenericTypeArguments[0];
                        resolutionCache[type] = resultType;
                    }

                    var data = resultType.IsPrimitive()
                        ? ReflectionExtension.ConvertTo(resultType, responseBody)
                        : JsonConvert.DeserializeObject(responseBody, resultType);

                    result = (TResult)Activator.CreateInstance(type, response.StatusCode, data);
                }

                return ServiceResponse<TResult>.CreateResponse(response, responseBody, result);
            }
            catch (Exception e)
            {
                throw new ServiceException(response, responseBody, e);
            }
        }
    }
}
