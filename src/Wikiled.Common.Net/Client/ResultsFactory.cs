using Wikiled.Common.Arguments;

namespace Wikiled.Common.Net.Client
{
    public static class ResultsFactory
    {
        public static ServiceResult<string> CreateErrorMessage(int code, string error)
        {
            Guard.IsValid(() => code, code, i => (i < 200) || (i > 299), "Only error code is acceptable");
            return new ServiceResult<string>(code, null, error);
        }
    }
}
