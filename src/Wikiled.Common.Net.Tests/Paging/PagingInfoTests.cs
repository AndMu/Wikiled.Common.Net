using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Common.Net.Paging;

namespace Wikiled.Common.Net.Tests.Paging;

[TestFixture]
public class PagingInfoTests
{
    [TestCase(1, 0, 9)]
    [TestCase(2, 10, 19)]
    public void ValidatePaging(int page, int start, int end)
    {
        var pagingInfo = new PagingInfo
        {
            PageNumber = page,
            PageSize = 10
        };

        ClassicAssert.AreEqual(start, pagingInfo.Start);
        ClassicAssert.AreEqual(end, pagingInfo.End);
    }
}