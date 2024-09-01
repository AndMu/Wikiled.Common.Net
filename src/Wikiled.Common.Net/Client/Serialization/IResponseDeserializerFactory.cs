namespace Wikiled.Common.Net.Client.Serialization;

public interface IResponseDeserializerFactory
{
    IResponseDeserializer Construct<T>();
}