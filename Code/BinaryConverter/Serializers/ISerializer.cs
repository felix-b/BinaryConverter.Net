using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Serializers
{
    public interface ISerializer
    {
        object Deserialize(BinaryTypesReader br, Type type, SerializerSettings settings, ISerializerArg serializerArg);

        void Serialize(BinaryTypesWriter bw, Type type, SerializerSettings settings, ISerializerArg serializerArg, object value);
    }

    public interface ISerializerArg
    {
    }

    public abstract class BaseSerializer : ISerializer
    {
        public abstract object Deserialize(BinaryTypesReader br, Type type, SerializerSettings settings, ISerializerArg serializerArg);

        public abstract void Serialize(BinaryTypesWriter bw, Type type, SerializerSettings settings, ISerializerArg serializerArg, object value);

        public TARG GetSerializerArg<TARG>(Type type, SerializerSettings settings, ISerializerArg serializerArg) where TARG : class, ISerializerArg
        {
            if (serializerArg == null && settings != null)
            {
                settings.SerializerArgMap.TryGetValue(type, out serializerArg);
            }

            if (serializerArg == null)
            {
                serializerArg = SerializerRegistry.GetSerializerArg(type);
            }

            return serializerArg as TARG;
        }
    }

}
