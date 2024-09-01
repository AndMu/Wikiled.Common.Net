using System;
using System.Collections.Concurrent;
using Wikiled.Common.Reflection;

namespace Wikiled.Common.Net.Client.Serialization;

public class ResponseDeserializerFactory : IResponseDeserializerFactory
{
    private static readonly ConcurrentDictionary<Type, IResponseDeserializer> ResolutionCache = new ConcurrentDictionary<Type, IResponseDeserializer>();

    private readonly RawResponseDeserializer rawResponseDeserializer = new RawResponseDeserializer();

    private readonly ResponseDeserializer responseDeserializer = new ResponseDeserializer();

    public IResponseDeserializer Construct<T>()
    {
        var type = typeof(T);
        if (!ResolutionCache.TryGetValue(type, out var result))
        {
            if (type.IsSubclassOfGeneric(typeof(RawResponse<>)))
            {
                result = rawResponseDeserializer;
            }
            else
            {
                result = responseDeserializer;
            }

            ResolutionCache[type] = result;
        }

        return result;
    }
}