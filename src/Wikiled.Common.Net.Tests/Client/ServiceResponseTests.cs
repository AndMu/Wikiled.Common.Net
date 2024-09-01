using System.Net;
using System.Net.Http;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Common.Net.Client;

namespace Wikiled.Common.Net.Tests.Client;

[TestFixture]
public class ServiceResponseTests
{
    [Test]
    public void ErrorResponse()
    {
        var message = new HttpResponseMessage(HttpStatusCode.BadRequest);
        var request = ServiceResponse<ServiceResult<TestData>>.CreateResponse(message, "Error", null);
        ClassicAssert.AreEqual(HttpStatusCode.BadRequest, request.HttpResponseMessage.StatusCode);
        ClassicAssert.IsFalse(request.IsSuccess);
    }

    [Test]
    public void GoodResponse()
    {
        var message = new HttpResponseMessage(HttpStatusCode.OK);
        var data = new ServiceResult<TestData> { StatusCode = 200, Value = new TestData(), Message = "TestMessage" };
        var request = ServiceResponse<ServiceResult<TestData>>.CreateResponse(message, "Test", data);
        ClassicAssert.AreEqual(HttpStatusCode.OK, request.HttpResponseMessage.StatusCode);
        ClassicAssert.AreEqual("TestMessage", request.Result.Message);
        ClassicAssert.IsTrue(request.IsSuccess);
        ClassicAssert.AreSame(data.Value, request.Result.Value);
    }
}