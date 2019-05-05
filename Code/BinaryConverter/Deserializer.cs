using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BinaryConverter
{
    public class DeserializerContext
    {
        public Type Type { get; set; }
        public BinaryTypesReader Reader { get; set; }
    }

    public static class Deserializer
    {
        public static T DeserializeObject<T>(byte[] buf)
        {
            return (T)DeserializeObject(buf, typeof(T));
        }

        public static object DeserializeObject(byte[] buf, Type type)
        {
            using (MemoryStream ms = new MemoryStream(buf))
            {

                using (BinaryTypesReader br = new BinaryTypesReader(ms))
                {
                    return DeserializeObject(br, type);
                }
            }
        }

        internal static object DeserializeObject(BinaryTypesReader br, Type type)
        {
            var serializer = SerializerRegistry.GetSerializer(type);

            if (serializer != null)
            {
                if (type.IsClass)
                {
                    if (br.ReadBoolean() == false) //null
                    {
                        return null;
                    }
                }

                return serializer.Deserialize(br, type);
            }

            if (type.IsEnum)
            {
                serializer = SerializerRegistry.GetSerializer(typeof(Enum));
                return serializer.Deserialize(br, type);
            }

            if (type.IsClass)
            {
                if (type.IsGenericType)
                {
                    serializer = SerializerRegistry.GetSerializer(type.GetGenericTypeDefinition());
                    if (serializer != null)
                    {
                        return serializer.Deserialize(br, type);
                    }
                }

                if (br.ReadBoolean() == false) //null
                {
                    return null;
                }

                serializer = SerializerRegistry.GetSerializer(typeof(object));
                return serializer.Deserialize(br, type);
            }

            throw new Exception($"DeserializeObject: serializer not found for type {type.FullName}");
        }





    }
}
