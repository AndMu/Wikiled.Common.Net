﻿using System;
using System.IO;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Wikiled.Common.Net.Client
{
    public class StreamApiClient : IStreamApiClient
    {
        private readonly Uri baseUri;

        private readonly HttpClient client;

        private readonly ILogger<StreamApiClient> logging;

        public StreamApiClient(HttpClient client, Uri baseUri, ILogger<StreamApiClient> logging)
        {
            this.baseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
            this.logging = logging ?? throw new ArgumentNullException(nameof(logging));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public IObservable<TResult> PostRequest<TInput, TResult>(string action, TInput argument, CancellationToken token)
        {
            return Observable.Create<TResult>(
                (observer, cancellationToken) =>
                    {
                        return ProcessStream(
                            () => client.PostAsync(new Uri(baseUri, action), new StringContent(JsonConvert.SerializeObject(argument), Encoding.UTF8, "application/json"), token),
                            cancellationToken,
                            observer);
                    });
        }

        public IObservable<TResult> GetRequest<TResult>(string action, CancellationToken token)
        {
            return Observable.Create<TResult>(
                (observer, cancellationToken) =>
                    {
                        return ProcessStream(() => client.GetAsync(new Uri(baseUri, action), cancellationToken), cancellationToken, observer);
                    });
        }

        private async Task ProcessStream<TResult>(Func<Task<HttpResponseMessage>> action,
            CancellationToken cancellationToken, IObserver<TResult> observer)
        {
            try
            {
                using (HttpResponseMessage response = await action().ConfigureAwait(false))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException(response.ToString());
                    }

                    using (Stream dataStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (StreamReader theStreamReader = new StreamReader(dataStream))
                    {
                        string theLine;
                        while ((theLine = theStreamReader.ReadLine()) != null ||
                               cancellationToken.IsCancellationRequested)
                        {
                            var data = JsonConvert.DeserializeObject<TResult>(theLine);
                            if (data == null)
                            {
                                logging.LogWarning("No Data received: {0}", theLine);
                            }
                            else
                            {
                                observer.OnNext(data);
                            }
                        }

                        response.Content = null;
                    }

                    observer.OnCompleted();
                }
            }
            catch (Exception e)
            {
                logging.LogError(e, "Streaming error");
                observer.OnError(e);
            }
        }
    }
}
