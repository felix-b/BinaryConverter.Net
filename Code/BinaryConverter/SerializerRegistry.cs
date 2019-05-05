using BinaryConverter.Serializers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter
{
    public static class SerializerRegistry
    {
        private static ConcurrentDictionary<Type, ISerializer> _serializerMap { get; set; } = new ConcurrentDictionary<Type, ISerializer>();

        public static void RegisterType(Type type, ISerializer serializer)
        {
            _serializerMap[type] = serializer;
        }

        public static ISerializer GetSerializer(Type type)
        {
            if (_serializerMap.TryGetValue(type, out var serializer))
            {
                return serializer;
            }

            if (type.IsEnum)
            {
                return null;//
            }

            return null;
        }

        static SerializerRegistry()
        {
            //register IntX
            _serializerMap[typeof(Boolean)] = new BooleanSerializer();
            _serializerMap[typeof(Byte)] = new ByteSerializer();
            _serializerMap[typeof(SByte)] = new SByteSerializer();
            _serializerMap[typeof(Int16)] = new Int16Serializer();
            _serializerMap[typeof(UInt16)] = new UInt16Serializer();
            _serializerMap[typeof(Int32)] = new Int32Serializer();
            _serializerMap[typeof(UInt32)] = new UInt32Serializer();
            _serializerMap[typeof(Int64)] = new Int64Serializer();
            _serializerMap[typeof(UInt64)] = new UInt64Serializer();
            _serializerMap[typeof(Char)] = new CharSerializer();

            //register DateTime
            _serializerMap[typeof(DateTime)] = new DateTimeSerializer();

            //register Decimal
            _serializerMap[typeof(decimal)] = new DecimalSerializer();

            //register Floating
            _serializerMap[typeof(double)] = new DoubleSerializer();
            _serializerMap[typeof(float)] = new SingleSerializer();

            //register Enum
            _serializerMap[typeof(Enum)] = new EnumSerializer();

            //register Collections
            _serializerMap[typeof(List<>)] = new ListSerializer();
            _serializerMap[typeof(Dictionary<,>)] = new DictionarySerializer();

            //register For String
            _serializerMap[typeof(string)] = new StringSerializer();

            //register For Class
            _serializerMap[typeof(object)] = new ClassSerializer();
        }
    }
}










