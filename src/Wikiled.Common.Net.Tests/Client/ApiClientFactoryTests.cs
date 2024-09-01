using System;
using System.Net.Http;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Common.Net.Client;

namespace Wikiled.Common.Net.Tests.Client;

[TestFixture]
public class ApiClientFactoryTests
{
    private HttpClient httpClient;

    private Uri baseUri;

    private ApiClientFactory instance;

    [SetUp]
    public void Setup()
    {
        httpClient = new HttpClient();
        baseUri = new Uri("http://test");
        instance = CreateFactory();
    }

    [Test]
    public void GetPostClient()
    {
        var client = instance.GetClient();
        ClassicAssert.IsNotNull(client);
    }

    [Test]
    public void Construct()
    {
        ClassicAssert.Throws<ArgumentNullException>(() => new ApiClientFactory(null, baseUri));
        ClassicAssert.Throws<ArgumentNullException>(() => new ApiClientFactory(httpClient, null));
    }

    private ApiClientFactory CreateFactory()
    {
        return new ApiClientFactory(httpClient, baseUri);
    }
}