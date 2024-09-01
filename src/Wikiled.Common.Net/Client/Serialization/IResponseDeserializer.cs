using System.Net.Http;
using System.Threading.Tasks;

namespace Wikiled.Common.Net.Client.Serialization;

public interface IResponseDeserializer
{
    Task<ServiceResponse<TResult>> GetData<TResult>(HttpResponseMessage response)
        where TResult : IApiResponse;
}