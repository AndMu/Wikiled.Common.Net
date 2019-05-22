using System;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using System.Net;
using RichardSzalay.MockHttp;
using Wikiled.Common.Net.Resilience;
using Wikiled.Common.Net.Tests.Helpers;

namespace Wikiled.Common.Net.Tests.Resilience
{
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

        [TestCase("http://ssssss", 6)]
        public void xxx(string uri, int result)
        {
            int total = 0;
            //var mockHttp = new MockHttpMessageHandler();
            //mockHttp.Expect("http://localhost/api/user/*")
            //        .Respond(HttpStatusCode.Unauthorized);
            //var httpClient = mockHttp.ToHttpClient();
            //var response = await httpClient.GetAsync("http://localhost/api/user/1234").ConfigureAwait(false);
            //response.
            //throw new WebException("Test", new Exception(), WebExceptionStatus.NameResolutionFailure, new HttpWebResponse());

            Assert.ThrowsAsync<WebException>(async () =>
                await instance.WebPolicy.ExecuteAsync(
                        async () =>
                        {
                            total++;
                            WebRequest.Create("http://ssssss").GetResponse();
                        })
                    .ConfigureAwait(false));
            Assert.AreEqual(result, total);
        }

        [TestCase(HttpStatusCode.Unauthorized, 1)]
        [TestCase(HttpStatusCode.BadRequest, 1)]
        [TestCase(HttpStatusCode.RequestTimeout, 6)]
        public void TestWebRequest(HttpStatusCode code, int result)
        {
            int total = 0;
            //var mockHttp = new MockHttpMessageHandler();
            //mockHttp.Expect("http://localhost/api/user/*")
            //        .Respond(HttpStatusCode.Unauthorized);
            //var httpClient = mockHttp.ToHttpClient();
            //var response = await httpClient.GetAsync("http://localhost/api/user/1234").ConfigureAwait(false);
            //response.
            //throw new WebException("Test", new Exception(), WebExceptionStatus.NameResolutionFailure, new HttpWebResponse());

            Assert.ThrowsAsync<WebException>(async () =>
                await instance.WebPolicy.ExecuteAsync( 
                                  async () =>
                                  {
                                      total++;
                                      throw new WebException("Test", new Exception(), WebExceptionStatus.Success, TestWebRequestCreate.CreateTestWebResponse("ss", code));
                                  })
                              .ConfigureAwait(false));
            Assert.AreEqual(result, total);
        }

        private CommonResilience CreateCommonResilience()
        {
            return new CommonResilience(new NullLogger<CommonResilience>(), config);
        }
    }
}
