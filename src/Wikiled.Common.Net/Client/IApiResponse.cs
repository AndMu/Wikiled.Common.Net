namespace Wikiled.Common.Net.Client;

public interface IApiResponse
{
    int StatusCode { get; }

    string Message { get; }
}