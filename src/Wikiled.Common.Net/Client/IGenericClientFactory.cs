using System;

namespace Wikiled.Common.Net.Client
{
    public interface IGenericClientFactory
    {
        IStreamApiClient ConstructStreaming(Uri url);

        IApiClient ConstructRegular(Uri url);

        IResilientApiClient ConstructResilient(Uri url);
    }
}