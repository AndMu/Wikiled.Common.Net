namespace Wikiled.Common.Net.Client
{
    public class ServiceResult<T> : IApiResponse
    {
        public T Value { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; }
    }
}
