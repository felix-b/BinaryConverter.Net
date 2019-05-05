using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Serializers
{
    public interface ISerializer
    {
        object Deserialize(BinaryTypesReader br, Type type);

        void Serialize(BinaryTypesWriter bw, Type type, object value);
    }

    public abstract class BaseSerializer : ISerializer
    {

        public abstract object Deserialize(BinaryTypesReader br, Type type);

        public abstract void Serialize(BinaryTypesWriter bw, Type type, object value);
    }
}
