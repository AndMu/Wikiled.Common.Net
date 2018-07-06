using System;
using NUnit.Framework;
using Wikiled.Common.Net.Client;

namespace Wikiled.Common.Net.Tests.Client
{
    [TestFixture]
    public class ResultsFactoryTests
    {
        [Test]
        public void CreateErrorMessage()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ResultsFactory.CreateErrorMessage(200, "Test"));
            var result = ResultsFactory.CreateErrorMessage(300, "Test");
            Assert.IsNotNull(result);
        }
    }
}