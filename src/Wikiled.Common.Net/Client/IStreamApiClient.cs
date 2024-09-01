using System;
using System.Threading;

namespace Wikiled.Common.Net.Client;

public interface IStreamApiClient
{
    IObservable<TResult> PostRequest<TInput, TResult>(string action, TInput argument, CancellationToken token);

    IObservable<TResult> GetRequest<TResult>(string action, CancellationToken token);
}