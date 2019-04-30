using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BinaryConverter
{
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

        private static object DeserializeObject(BinaryTypesReader br, Type type)
        {
            if (type.IsPrimitive)
            {
                switch (type.FullName)
                {
                    case SystemTypeDefs.FullNameBoolean:
                    case SystemTypeDefs.FullNameByte:
                    case SystemTypeDefs.FullNameSByte:
                    case SystemTypeDefs.FullNameInt16:
                    case SystemTypeDefs.FullNameUInt16:
                    case SystemTypeDefs.FullNameInt32:
                    case SystemTypeDefs.FullNameUInt32:
                    case SystemTypeDefs.FullNameInt64:
                    case SystemTypeDefs.FullNameUInt64:
                    case SystemTypeDefs.FullNameChar:
                        {
                            var val = br.Read7BitLong();
                            return Convert.ChangeType(val, type);
                        }
                    case SystemTypeDefs.FullNameSingle: // todo: compact
                        return br.ReadSingle();
                    case SystemTypeDefs.FullNameDouble: // todo: compact
                        return br.ReadDouble();

                }
            }

            if (type.FullName == typeof(decimal).FullName)
            {
                return br.ReadCompactDecimal();
            }

            if (type.IsEnum)
            {
                int val = (int)br.Read7BitLong();
                return Enum.ToObject(type, val);
            }

            if (type.IsValueType)
            {
                switch (type.FullName)
                {
                    case SystemTypeDefs.FullNameDateTime:
                        return new DateTime(br.Read7BitLong(), DateTimeKind.Utc);
                }
            }

            if (type.IsClass)
            {
                if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)))
                {
                    return DeserializeGenericList(br, type);
                }
                if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Dictionary<,>)))
                {
                    return DeserializeGenericDictionary(br, type);
                }

                bool isNotNull = br.ReadBoolean();

                if (isNotNull == false)
                {
                    return null;
                }

                switch (type.FullName)
                {
                    case SystemTypeDefs.FullNameString:
                        return br.ReadString();
                    default:
                        return DeserializeClass(br, type);
                }
            }

            return null;
        }


        private static object DeserializeGenericList(BinaryTypesReader br, Type type)
        {
            int count = br.Read7BitInt();
            if (count == -1)
            {
                return null;
            }
            var instance = (IList)Activator.CreateInstance(type);
            Type genericArgtype = type.GetGenericArguments()[0];

            for (int i = 0; i < count; i++)
            {
                instance.Add(DeserializeObject(br, genericArgtype));
            }
            return instance;
        }


        private static object DeserializeGenericDictionary(BinaryTypesReader br, Type type)
        {
            int count = br.Read7BitInt();
            if (count == -1)
            {
                return null;
            }
            var instance = (IDictionary)Activator.CreateInstance(type);
            Type genericArgKey = type.GetGenericArguments()[0];
            Type genericArgVal = type.GetGenericArguments()[1];

            for (int i = 0; i < count; i++)
            {
                instance.Add(DeserializeObject(br, genericArgKey), DeserializeObject(br, genericArgVal));
            }
            return instance;
        }

        private static object DeserializeClass(BinaryTypesReader br, Type type)
        {
            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .OrderBy(x => x.MetadataToken);

            var instance = Activator.CreateInstance(type);


            foreach (var prop in props)
            {
                prop.SetValue(instance, DeserializeObject(br, prop.PropertyType));
            }

            return instance;
        }
    }
}
