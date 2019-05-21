using System.Net;
using System.Net.Http;
using NUnit.Framework;
using Wikiled.Common.Net.Client;

namespace Wikiled.Common.Net.Tests.Client
{
    [TestFixture]
    public class ServiceResponseTests
    {
        [Test]
        public void ErrorResponse()
        {
            var message = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var request = ServiceResponse<ServiceResult<TestData>>.CreateResponse(message, "Error", null);
            Assert.AreEqual(HttpStatusCode.BadRequest, request.HttpResponseMessage.StatusCode);
            Assert.IsFalse(request.IsSuccess);
        }

        [Test]
        public void GoodResponse()
        {
            var message = new HttpResponseMessage(HttpStatusCode.OK);
            var data = new ServiceResult<TestData>(200, new TestData(), "TestMessage");
            var request = ServiceResponse<ServiceResult<TestData>>.CreateResponse(message, "Test", data);
            Assert.AreEqual(HttpStatusCode.OK, request.HttpResponseMessage.StatusCode);
            Assert.AreEqual("TestMessage", request.Result.Message);
            Assert.IsTrue(request.IsSuccess);
            Assert.AreSame(data.Value, request.Result.Value);
        }
    }
}
