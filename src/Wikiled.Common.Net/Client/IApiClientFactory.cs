namespace Wikiled.Common.Net.Client
{
    public interface IApiClientFactory
    {
        IApiClient GetClient();
    }
}