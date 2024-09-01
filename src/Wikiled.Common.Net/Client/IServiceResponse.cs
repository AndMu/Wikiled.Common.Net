using System.Net.Http;

namespace Wikiled.Common.Net.Client;

public interface IServiceResponse
{
    HttpResponseMessage HttpResponseMessage { get; }

    string Body { get; }

    bool IsSuccess { get; }

    TResult ReadAs<TResult>();
}