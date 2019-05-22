using Moq;
using System.IO;
using System.Net;

namespace Wikiled.Common.Net.Tests.Helpers
{
    internal class TestWebRequestCreate
    {
        public static WebResponse CreateTestWebResponse(string responseStr, HttpStatusCode code)
        {
            var response = new Mock<HttpWebResponse>();
            response.Setup(item => item.StatusCode).Returns(code);
            var responseStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(responseStr));
            response.Setup(x => x.GetResponseStream()).Returns(responseStream);
            return response.Object;
        }
    }
}
