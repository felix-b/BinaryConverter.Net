using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Serializers
{
    public interface ISerializer
    {
        bool CommonNullHandle { get; }

        object Deserialize(BinaryTypesReader br, Type type, SerializerSettings settings, ISerializerArg serializerArg);

        void Serialize(BinaryTypesWriter bw, Type type, SerializerSettings settings, ISerializerArg serializerArg, object value);
    }

    public interface ISerializerArg
    {
    }

    public abstract class BaseSerializer : ISerializer
    {
        public virtual bool CommonNullHandle { get { return true; } }

        public abstract object Deserialize(BinaryTypesReader br, Type type, SerializerSettings settings, ISerializerArg serializerArg);

        public abstract void Serialize(BinaryTypesWriter bw, Type type, SerializerSettings settings, ISerializerArg serializerArg, object value);

        public TArg GetSerializerArg<TArg>(Type type, SerializerSettings settings, ISerializerArg serializerArg) where TArg : class, ISerializerArg
        {
            if (serializerArg == null && settings != null)
            {
                settings.SerializerArgMap.TryGetValue(type, out serializerArg);
            }

            if (serializerArg == null)
            {
                serializerArg = SerializerRegistry.GetSerializerArg(type);
            }

            return serializerArg as TArg;
        }
    }

}
