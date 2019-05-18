﻿using BinaryConverter.Serializers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter
{

    public static class SerializerRegistry
    {
        private static ConcurrentDictionary<Type, ISerializer> _serializerMap { get; set; } = new ConcurrentDictionary<Type, ISerializer>();
        private static ConcurrentDictionary<Type, ISerializerArg> _serializerArgMap { get; set; } = new ConcurrentDictionary<Type, ISerializerArg>();
        private static ConcurrentDictionary<Type, IClassMap> _classMapMap { get; set; } = new ConcurrentDictionary<Type, IClassMap>();

        public static void RegisterSerializer(Type type, ISerializer serializer)
        {
            _serializerMap[type] = serializer;
        }

        public static ISerializer GetSerializer(Type type)
        {
            _serializerMap.TryGetValue(type, out var serializer);
            return serializer;

        }

        public static void RegisterSerializerArg(Type type, ISerializerArg serializerArg)
        {
            _serializerArgMap[type] = serializerArg;
        }

        public static ISerializerArg GetSerializerArg(Type type)
        {
            _serializerArgMap.TryGetValue(type, out var serializerArg);
            return serializerArg;
        }

        public static void RegisterClassMap(Type type, IClassMap classMap)
        {
            _classMapMap[type] = classMap;
        }

        public static IClassMap GetClassMap(Type type)
        {
            _classMapMap.TryGetValue(type, out var classMap);
            return classMap;
        }

        static SerializerRegistry()
        {
            //register IntX
            RegisterSerializer(typeof(Boolean), new BooleanSerializer());
            RegisterSerializer(typeof(Byte), new ByteSerializer());
            RegisterSerializer(typeof(SByte), new SByteSerializer());
            RegisterSerializer(typeof(Int16), new Int16Serializer());
            RegisterSerializer(typeof(UInt16), new UInt16Serializer());
            RegisterSerializer(typeof(Int32), new Int32Serializer());
            RegisterSerializer(typeof(UInt32), new UInt32Serializer());
            RegisterSerializer(typeof(Int64), new Int64Serializer());
            RegisterSerializer(typeof(UInt64), new UInt64Serializer());
            RegisterSerializer(typeof(Char), new CharSerializer());

            //register DateTime
            RegisterSerializer(typeof(DateTime), new DateTimeSerializer());
            RegisterSerializerArg(typeof(DateTime), new DateTimeSerializerArg() { TickResolution = TimeSpan.TicksPerSecond});

            //register Decimal
            RegisterSerializer(typeof(decimal), new DecimalSerializer());

            //register Floating
            RegisterSerializer(typeof(double), new DoubleSerializer());
            //RegisterSerializerArg(typeof(double), new FloatingPointSerializerArg() { DecimalDigits = 4});
            RegisterSerializer(typeof(float), new SingleSerializer());
            //RegisterSerializerArg(typeof(float), new FloatingPointSerializerArg() { DecimalDigits = 4});

            //register Enum
            RegisterSerializer(typeof(Enum), new EnumSerializer());

            //register Collections
            RegisterSerializer(typeof(List<>), new ListSerializer());
            RegisterSerializer(typeof(Dictionary<,>), new DictionarySerializer());

            //register For String
            RegisterSerializer(typeof(string), new StringSerializer());

            //register For Class
            RegisterSerializer(typeof(object), new ClassSerializer());
        }
    }
}










