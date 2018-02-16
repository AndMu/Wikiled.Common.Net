namespace Wikiled.Common.Net.Client
{
    public class RawResponse<T> : IApiResponse
    {
        public RawResponse(int code, T value)
        {
            StatusCode = code;
            Value = value;
        }

        public T Value { get; }

        public int StatusCode { get; }

        public string Message { get; } = "";
    }
}
