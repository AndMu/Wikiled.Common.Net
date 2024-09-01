namespace Wikiled.Common.Net.Client;

public interface IStreamApiClientFactory
{
    IStreamApiClient Construct();
}