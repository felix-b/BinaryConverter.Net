using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BinaryConverter
{

    public static class Serializer
    {
        public static byte[] SerializeObject<T>(T value)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryTypesWriter bw = new BinaryTypesWriter(ms))
                {
                    SerializeObject(typeof(T), value, bw);
                    return ms.ToArray();
                }
            }
        }

        internal static void SerializeObject<T>(Type type, T value, BinaryTypesWriter bw)
        {

            var serializer = SerializerRegistry.GetSerializer(type);

            if (serializer != null)
            {
                if (type.IsClass)
                {
                    bw.Write(value != null);
                    if (value == null)
                    {
                        return;
                    }
                }

                serializer.Serialize(bw, type, value);
                return;
            }

            if (type.IsEnum)
            {
                serializer = SerializerRegistry.GetSerializer(typeof(Enum));
                serializer.Serialize(bw, type, value);
                return;
            }

            if (type.IsClass)
            {
                if (type.IsGenericType)
                {
                    serializer = SerializerRegistry.GetSerializer(type.GetGenericTypeDefinition());
                    if (serializer != null)
                    {
                        serializer.Serialize(bw, type, value);
                        return;
                    }
                }

                bw.Write(value != null);
                if (value == null)
                {
                    return;
                }

                serializer = SerializerRegistry.GetSerializer(typeof(object));
                serializer.Serialize(bw, type, value);
                return;
            }

            throw new Exception($"SerializeObject: serializer not found for type {type.FullName}");

        }

    }
}
