using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Serializers
{
    class EnumSerializer : BaseSerializer
    {
        public override object Deserialize(BinaryTypesReader br, Type type, SerializerSettings settings, ISerializerArg serializerArg)
        {
            int val = (int)br.Read7BitLong();
            return Enum.ToObject(type, val);
        }

        public override void Serialize(BinaryTypesWriter bw, Type type, SerializerSettings settings, ISerializerArg serializerArg, object value)
        {
            bw.Write7BitLong((int)(object)value);
        }
    }
}
