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

        private static void SerializeObject<T>(Type type, T value, BinaryTypesWriter bw)
        {
            //var type = value.GetType();

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
                            var val = Convert.ChangeType(value, typeof(long));
                            bw.Write7BitLong((long)val);
                            break;
                        }

                    case SystemTypeDefs.FullNameSingle: // todo: compact
                        bw.Write((float)(object)value);
                        break;
                    case SystemTypeDefs.FullNameDouble: // todo: compact
                        bw.Write((double)(object)value);
                        break;
                }
                return;
            }

            if (type.FullName == SystemTypeDefs.FullNameDecimal)
            {
                bw.WriteCompactDecimal((decimal)(object)value);
                return;
            }

            if (type.IsEnum)
            {
                bw.Write7BitLong((int)(object)value);
                return;
            }

            if (type.IsValueType)
            {
                switch (type.FullName)
                {
                    case SystemTypeDefs.FullNameDateTime:
                        bw.Write7BitLong(((DateTime)(object)value).Ticks);
                        break;
                }
                return;
            }

            if (type.IsClass)
            {
                if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)))
                {
                    SerializeGenericList(type, (IList)value, bw);
                    return;
                }

                if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Dictionary<,>)))
                {
                    SerializeGenericDictionary(type, (IDictionary)value, bw);
                    return;
                }

                bool isNotNull = value != null;
                bw.Write(isNotNull);

                if (isNotNull == false)
                {
                    return;
                }

                switch (type.FullName)
                {
                    case SystemTypeDefs.FullNameString:
                        bw.Write((string)(object)value);
                        break;
                    default:
                        SerializeClass((object)value, bw);
                        break;
                }
                return;
            }

        }


        private static void SerializeGenericList<T>(Type type, T value, BinaryTypesWriter bw) where T : IList
        {
            if (value == null)
            {
                bw.Write7BitLong(-1);
                return;
            }
            var count = value.Count;
            bw.Write7BitLong(count);
            Type genericArgtype = type.GetGenericArguments()[0];
            for (int i = 0; i < count; i++)
            {
                SerializeObject(genericArgtype, value[i], bw);
            }
        }


        private static void SerializeGenericDictionary(Type type, IDictionary value, BinaryTypesWriter bw)
        {
            if (value == null)
            {
                bw.Write7BitLong(-1);
                return;
            }
            var count = value.Count;
            bw.Write7BitLong(count);
            Type genericArgKey = type.GetGenericArguments()[0];
            Type genericArgVal = type.GetGenericArguments()[1];
            foreach (var key in value.Keys)
            {
                SerializeObject(genericArgKey, key, bw);
                SerializeObject(genericArgVal, value[key], bw);
            }
        }

        private static void SerializeClass<T>(T value, BinaryTypesWriter bw) where T : class
        {
            var props = value.GetType().GetProperties()
                .OrderBy(x => x.MetadataToken)
                .ToList();

            foreach (var prop in props)
            {
                var val = prop.GetValue(value);
                SerializeObject(prop.PropertyType, val, bw);
            }

        }
    }
}
