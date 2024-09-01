using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework.Legacy;
using RichardSzalay.MockHttp;
using Wikiled.Common.Net.Client;
using Wikiled.Common.Net.Client.Serialization;
using Wikiled.Common.Net.Paging;

namespace Wikiled.Common.Net.Tests.Client;

[TestFixture]
public class PagingApiClientExtensionTests
{

    private HttpClient httpClient;

    private ApiClient instance;

    private MockHttpMessageHandler mockHttp;

    private Uri baseUri;

    [SetUp]
    public void Setup()
    {
        mockHttp = new MockHttpMessageHandler();
        httpClient = new HttpClient(mockHttp);
        baseUri = new Uri("http://localhost");
        instance = CreatePostApiClient();
    }

    [Test]
    public async Task PostPagingRequest()
    {
        // Setup a respond for the user api (including a wildcard in the URL)
        mockHttp.When("http://localhost/test")
            .Respond(request =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("[{\"Name\" : \"Test Result\"}]", Encoding.UTF8, "application/json")
                };

                response.Headers.Add(PagingConstants.TotalHeader, "1");

                return response;
            });
        var result = await instance.PostPagingRequest<TestData>("test", new PagingInfo(), CancellationToken.None).ConfigureAwait(false);
        ClassicAssert.AreEqual(1, result.TotalItems);
        ClassicAssert.AreEqual("Test Result", result.Data[0].Name);
    }

    private ApiClient CreatePostApiClient()
    {
        return new ApiClient(httpClient, baseUri, new ResponseDeserializerFactory());
    }
}