using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using RichardSzalay.MockHttp;
using Wikiled.Common.Net.Client;
using Wikiled.Common.Net.Client.Serialization;

namespace Wikiled.Common.Net.Tests.Client;

[TestFixture]
public class ApiClientTests
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
    public async Task PostRequest()
    {
        // Setup a respond for the user api (including a wildcard in the URL)
        mockHttp.When("http://localhost/test")
            .Respond("application/json", "{ \"Value\": {\"Name\" : \"Test Result\"}, \"StatusCode\": 200}");
        var argument  = new TestData();
        var result = await instance.PostRequest<TestData, ServiceResult<TestData>>("test", argument, CancellationToken.None).ConfigureAwait(false);
        ClassicAssert.IsTrue(result.IsSuccess);
        ClassicAssert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);
        ClassicAssert.AreEqual("Test Result", result.Result.Value.Name);
    }

    [Test]
    public async Task GetRequest()
    {
        mockHttp.When("http://localhost/test/argument")
            .Respond("application/json", "{ \"Value\": {\"Name\" : \"Test Result\"}, \"StatusCode\": 200}");
        var result = await instance.GetRequest<ServiceResult<TestData>>("test/argument", CancellationToken.None).ConfigureAwait(false);
        ClassicAssert.IsTrue(result.IsSuccess);
        ClassicAssert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);
        ClassicAssert.AreEqual("Test Result", result.Result.Value.Name);
    }

    [Test]
    public async Task GetRequestString()
    {
        mockHttp.When("http://localhost/test/argument")
            .Respond("application/json", "{ \"Value\": \"Test Result\", \"StatusCode\": 200}");
        var result = await instance.GetRequest<ServiceResult<string>>("test/argument", CancellationToken.None).ConfigureAwait(false);
        ClassicAssert.IsTrue(result.IsSuccess);
        ClassicAssert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);
        ClassicAssert.AreEqual("Test Result", result.Result.Value);
    }

    [Test]
    public async Task GetRequestInt()
    {
        mockHttp.When("http://localhost/test/argument")
            .Respond("application/json", "{ \"Value\": 1, \"StatusCode\": 200}");
        var result = await instance.GetRequest<ServiceResult<int>>("test/argument", CancellationToken.None).ConfigureAwait(false);
        ClassicAssert.IsTrue(result.IsSuccess);
        ClassicAssert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);
        ClassicAssert.AreEqual(1, result.Result.Value);
    }

    [Test]
    public async Task GetRequestBool()
    {
        mockHttp.When("http://localhost/test/argument")
            .Respond("application/json", "{ \"Value\": true, \"StatusCode\": 200}");
        var result = await instance.GetRequest<ServiceResult<bool>>("test/argument", CancellationToken.None).ConfigureAwait(false);
        ClassicAssert.IsTrue(result.IsSuccess);
        ClassicAssert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);
        ClassicAssert.AreEqual(true, result.Result.Value);
        ClassicAssert.AreEqual(true, result.ReadAs<ServiceResult<bool>>().Value);
    }

    [Test]
    public async Task GetRawRequest()
    {
        instance = new ApiClient(httpClient, baseUri, new ResponseDeserializerFactory());
        mockHttp.When("http://localhost/test/argument")
            .Respond("application/json", "{ \"Name\" : \"Test Result\"}");

        var result = await instance.GetRequest<RawResponse<TestData>>("test/argument", CancellationToken.None).ConfigureAwait(false);
        ClassicAssert.IsTrue(result.IsSuccess);
        ClassicAssert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);
        ClassicAssert.AreEqual("Test Result", result.Result.Value.Name);
        ClassicAssert.AreEqual("Test Result", result.ReadAs<TestData>().Name);
    }

    [Test]
    public async Task GetRawRequestString()
    {
        instance = new ApiClient(httpClient, baseUri, new ResponseDeserializerFactory());
        mockHttp.When("http://localhost/test/argument")
            .Respond("application/text", "Test Result");

        var result = await instance.GetRequest<RawResponse<string>>("test/argument", CancellationToken.None).ConfigureAwait(false);
        ClassicAssert.IsTrue(result.IsSuccess);
        ClassicAssert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage.StatusCode);
        ClassicAssert.AreEqual("Test Result", result.Result.Value);
    }

    [Test]
    public void Construct()
    {
        ClassicAssert.Throws<ArgumentNullException>(() => new ApiClient(null, baseUri, new ResponseDeserializerFactory()));
        ClassicAssert.Throws<ArgumentNullException>(() => new ApiClient(httpClient, null, new ResponseDeserializerFactory()));
        ClassicAssert.Throws<ArgumentNullException>(() => new ApiClient(null, null, new ResponseDeserializerFactory()));
    }

    private ApiClient CreatePostApiClient()
    {
        return new ApiClient(httpClient, baseUri, new ResponseDeserializerFactory());
    }
}