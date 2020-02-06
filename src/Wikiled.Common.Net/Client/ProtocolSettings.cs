using System.Text.Json;

namespace Wikiled.Common.Net.Client
{
    public static class ProtocolSettings
    {
        public static JsonSerializerOptions SerializerOptions { get; } = new JsonSerializerOptions
        {
            IgnoreNullValues = true,
            IgnoreReadOnlyProperties = true
        };
    }
}
