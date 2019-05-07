using BinaryConverter.Serializers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BinaryConverter
{

    public class SerializerSettings
    {
        public Dictionary<Type, ISerializer> SerializerMap { get; set; } = new Dictionary<Type, ISerializer>();
        public Dictionary<Type, ISerializerArg> SerializerArgMap { get; set; } = new Dictionary<Type, ISerializerArg>();
    }


    public static class Serializer
    {
        //===============================================================================================
        // Serialize
        //===============================================================================================

        public static byte[] SerializeObject<T>(T value, SerializerSettings settings)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryTypesWriter bw = new BinaryTypesWriter(ms))
                {
                    SerializeObject(typeof(T), value, bw, settings, null);
                    return ms.ToArray();
                }
            }
        }

        internal static void SerializeObject<T>(Type type, T value, BinaryTypesWriter bw, SerializerSettings settings, ISerializer serializer)
        {
            if (serializer == null)
            {
                serializer = SerializerRegistry.GetSerializer(type);
            }

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

                serializer.Serialize(bw, type, settings, null, value);
                return;
            }

            if (type.IsEnum)
            {
                serializer = SerializerRegistry.GetSerializer(typeof(Enum));
                serializer.Serialize(bw, type, settings, null, value);
                return;
            }

            if (type.IsClass)
            {
                if (type.IsGenericType)
                {
                    serializer = SerializerRegistry.GetSerializer(type.GetGenericTypeDefinition());
                    if (serializer != null)
                    {
                        serializer.Serialize(bw, type, settings, null, value);
                        return;
                    }
                }

                bw.Write(value != null);
                if (value == null)
                {
                    return;
                }

                serializer = SerializerRegistry.GetSerializer(typeof(object));
                serializer.Serialize(bw, type, settings, null, value);
                return;
            }

            throw new Exception($"SerializeObject: serializer not found for type {type.FullName}");

        }

        //===============================================================================================
        // Deserialize
        //===============================================================================================


        public static T DeserializeObject<T>(byte[] buf, SerializerSettings settings)
        {
            return (T)DeserializeObject(buf, typeof(T), settings);
        }

        public static object DeserializeObject(byte[] buf, Type type, SerializerSettings settings)
        {
            using (MemoryStream ms = new MemoryStream(buf))
            {

                using (BinaryTypesReader br = new BinaryTypesReader(ms))
                {
                    return DeserializeObject(br, type, null, null);
                }
            }
        }

        internal static object DeserializeObject(BinaryTypesReader br, Type type, SerializerSettings settings, ISerializer serializer)
        {
            if (serializer == null)
            {
                serializer = SerializerRegistry.GetSerializer(type);
            }

            if (serializer != null)
            {
                if (type.IsClass)
                {
                    if (br.ReadBoolean() == false) //null
                    {
                        return null;
                    }
                }

                return serializer.Deserialize(br, type, settings, null);
            }

            if (type.IsEnum)
            {
                serializer = SerializerRegistry.GetSerializer(typeof(Enum));
                return serializer.Deserialize(br, type, settings, null);
            }

            if (type.IsClass)
            {
                if (type.IsGenericType)
                {
                    serializer = SerializerRegistry.GetSerializer(type.GetGenericTypeDefinition());
                    if (serializer != null)
                    {
                        return serializer.Deserialize(br, type, settings, null);
                    }
                }

                if (br.ReadBoolean() == false) //null
                {
                    return null;
                }

                serializer = SerializerRegistry.GetSerializer(typeof(object));
                return serializer.Deserialize(br, type, settings, null);
            }

            throw new Exception($"DeserializeObject: serializer not found for type {type.FullName}");
        }

        //===============================================================================================
        // 
        //===============================================================================================

    }
}
