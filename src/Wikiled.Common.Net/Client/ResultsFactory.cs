using System;

namespace Wikiled.Common.Net.Client;

public static class ResultsFactory
{
    public static ServiceResult<string> CreateErrorMessage(int code, string error)
    {
        if (code >= 200 && code < 300)
        {
            throw new ArgumentOutOfRangeException("Only error code is acceptable", nameof(code));
        }

        return new ServiceResult<string>
        {
            StatusCode = code,
            Message = error
        };
    }
}