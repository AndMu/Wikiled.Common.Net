using System;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using Wikiled.Common.Net.Resilience;

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

        [TestCase(WebExceptionStatus.Timeout, 6)]
        [TestCase(WebExceptionStatus.ConnectFailure, 6)]
        public void TestWeb(WebExceptionStatus code, int result)
        {
            int total = 0;
            Assert.ThrowsAsync<WebException>(async () =>
                await instance.WebPolicy.ExecuteAsync(
                        () =>
                        {
                            new WebResponse()
                            total++;
                            throw new WebException("Error", new Exception(), code, new HttpWebResponse());
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
