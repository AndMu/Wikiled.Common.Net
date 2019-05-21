using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using Wikiled.Common.Net.Resilience;

namespace Wikiled.Common.Net.Tests.Resilience
{
    [TestFixture]
    public class CommonResilienceTests
    {
        private CommonResilience instance;

        [SetUp]
        public void SetUp()
        {
            instance = CreateCommonResilience();
        }

        [TestCase(WebExceptionStatus.Timeout, 2)]
        public async Task TestWeb(WebExceptionStatus code, int result)
        {
            int total = 0;
            await instance.WebPolicy.ExecuteAsync(
                    () =>
                    {
                        result++;
                        throw new WebException("Error", code);
                    })
                .ConfigureAwait(false);
            Assert.AreEqual(result, total);
        }

        private CommonResilience CreateCommonResilience()
        {
            return new CommonResilience(new NullLogger<CommonResilience>(), ResilienceConfig.GenerateCommon());
        }
    }
}
