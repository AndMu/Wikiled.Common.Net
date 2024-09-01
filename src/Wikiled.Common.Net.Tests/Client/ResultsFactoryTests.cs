using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Common.Net.Client;

namespace Wikiled.Common.Net.Tests.Client;

[TestFixture]
public class ResultsFactoryTests
{
    [Test]
    public void CreateErrorMessage()
    {
        ClassicAssert.Throws<ArgumentOutOfRangeException>(() => ResultsFactory.CreateErrorMessage(200, "Test"));
        var result = ResultsFactory.CreateErrorMessage(300, "Test");
        ClassicAssert.IsNotNull(result);
    }
}