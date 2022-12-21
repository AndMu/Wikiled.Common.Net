using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wikiled.Common.Net.Client
{
    public static class ProtocolSettings
    {
        public static JsonSerializerOptions SerializerOptions { get; } = new ()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            IgnoreReadOnlyProperties = true,
            PropertyNameCaseInsensitive = true
        };
    }
}
