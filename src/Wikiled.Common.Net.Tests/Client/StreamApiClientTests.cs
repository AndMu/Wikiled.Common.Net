using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using Wikiled.Common.Net.Client;

namespace Wikiled.Common.Net.Tests.Client
{
    [TestFixture]
    public class StreamApiClientTests
    {
        private HttpClient httpClient;

        private StreamApiClient instance;

        private MockHttpMessageHandler mockHttp;

        private Uri baseUri;

        [SetUp]
        public void Setup()
        {
            mockHttp = new MockHttpMessageHandler();
            httpClient = new HttpClient(mockHttp);
            baseUri = new Uri("http://localhost");
            instance = CreateStreamApiClient();
        }

        [Test]
        public async Task GetRawRequest()
        {
            var item = "{ 'name' : 'Test Result'}";
            mockHttp.When("http://localhost/test/argument")
                    .Respond("application/json", item + Environment.NewLine + item);

            var result = await instance.GetRequest<TestData>(
                             "test/argument",
                             CancellationToken.None).ToArray();
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("Test Result", result[0].Name);
        }


        [Test]
        public async Task PostRequest()
        {
            var item = "{ 'name' : 'Test Result'}";
            mockHttp.When("http://localhost/test")
                .Respond("application/json", item + Environment.NewLine + item);
            TestData argument = new TestData();
            var result = await instance.PostRequest<TestData, TestData>("test", argument, CancellationToken.None).ToArray();
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("Test Result", result[0].Name);
        }

        [Test]
        public void Construct()
        {
            Assert.Throws<ArgumentNullException>(() => new StreamApiClient(null, baseUri));
            Assert.Throws<ArgumentNullException>(() => new StreamApiClient(httpClient, null));
        }

        private StreamApiClient CreateStreamApiClient()
        {
            return new StreamApiClient(httpClient, baseUri);
        }
    }
}