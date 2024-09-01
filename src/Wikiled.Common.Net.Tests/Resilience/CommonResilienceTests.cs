using System;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using System.Net;
using System.Threading;
using NUnit.Framework.Legacy;
using RichardSzalay.MockHttp;
using Wikiled.Common.Net.Client;
using Wikiled.Common.Net.Client.Serialization;
using Wikiled.Common.Net.Resilience;
using Wikiled.Common.Net.Tests.Client;
using Wikiled.Common.Net.Tests.Helpers;

namespace Wikiled.Common.Net.Tests.Resilience;

[TestFixture]
public class CommonResilienceTests
{
    private CommonResilience instance;

    private ResilienceConfig config;

    [SetUp]
    public void SetUp()
    {
        config = (ResilienceConfig)ResilienceConfig.GenerateCommon();
        config.ShortDelay = 1;
        config.LongDelay = 1;
        instance = CreateCommonResilience();
    }

    [Test]
    public void TestBadContent()
    {
        int total = 0;
        ClassicAssert.ThrowsAsync<ServiceException>(async () =>
            await instance.WebPolicy.ExecuteAsync(
                    async () =>
                    {
                        total++;
                        var mockHttp = new MockHttpMessageHandler();
                        mockHttp.Expect("http://localhost/api/user/*")
                            .Respond("application/json",
                                "{ asdas dasdasdasd }");
                        var client = new ApiClient(mockHttp.ToHttpClient(), new Uri("http://localhost/api/user/"), new ResponseDeserializerFactory());
                        await client.GetRequest<RawResponse<TestData>>("Test", CancellationToken.None).ProcessResult().ConfigureAwait(false);
                    })
                .ConfigureAwait(false));
        ClassicAssert.AreEqual(1, total);
    }

    [TestCase(HttpStatusCode.Unauthorized, 1)]
    [TestCase(HttpStatusCode.RequestTimeout, 6)]
    public void TestServiceException(HttpStatusCode code, int result)
    {
        int total = 0;
        ClassicAssert.ThrowsAsync<ServiceException>(async () =>
            await instance.WebPolicy.ExecuteAsync(
                    async () =>
                    {
                        total++;
                        var mockHttp = new MockHttpMessageHandler();
                        mockHttp.Expect("http://localhost/api/user/*")
                            .Respond(code);
                        var client = new ApiClient(mockHttp.ToHttpClient(), new Uri("http://localhost/api/user/"), new ResponseDeserializerFactory());
                        await client.GetRequest<RawResponse<string>>("Test", CancellationToken.None).ProcessResult().ConfigureAwait(false);
                    })
                .ConfigureAwait(false));
        ClassicAssert.AreEqual(result, total);
    }

    [TestCase(HttpStatusCode.Unauthorized, 1)]
    [TestCase(HttpStatusCode.BadRequest, 1)]
    [TestCase(HttpStatusCode.RequestTimeout, 6)]
    public void TestWebRequest(HttpStatusCode code, int result)
    {
        int total = 0;
        ClassicAssert.ThrowsAsync<WebException>(async () =>
            await instance.WebPolicy.ExecuteAsync(
                    () =>
                    {
                        total++;
                        throw new WebException("Test", new Exception(), WebExceptionStatus.Success, TestWebRequestCreate.CreateTestWebResponse("ss", code));
                    })
                .ConfigureAwait(false));
        ClassicAssert.AreEqual(result, total);
    }

    private CommonResilience CreateCommonResilience()
    {
        return new CommonResilience(new NullLogger<CommonResilience>(), config);
    }
}