using System;
using System.Collections.Concurrent;
using Wikiled.Common.Reflection;

namespace Wikiled.Common.Net.Client.Serialization
{
    public class ResponseDeserializerFactory : IResponseDeserializerFactory
    {
        private readonly RawResponseDeserializer rawResponseDeserializer = new RawResponseDeserializer();

        private static readonly ConcurrentDictionary<Type, IResponseDeserializer> resolutionCache = new ConcurrentDictionary<Type, IResponseDeserializer>();

        private readonly ResponseDeserializer responseDeserializer = new ResponseDeserializer();

        public IResponseDeserializer Construct<T>()
        {
            var type = typeof(T);
            if (!resolutionCache.TryGetValue(type, out var result))
            {
                if (type.IsSubclassOfGeneric(typeof(RawResponse<>)))
                {
                    result = rawResponseDeserializer;
                }
                else
                {
                    result = responseDeserializer;
                }

                resolutionCache[type] = result;
            }

            return result;
        }
    }
}
