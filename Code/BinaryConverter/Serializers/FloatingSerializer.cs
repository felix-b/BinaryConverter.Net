using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Serializers
{
    class DoubleSerializer : BaseSerializer
    {
        public override object Deserialize(BinaryTypesReader br, Type type)
        {
            return br.ReadDouble(); //todo: compact
        }

        public override void Serialize(BinaryTypesWriter bw, Type type, object value)
        {
            bw.Write((double)(object)value);
        }
    }

    class SingleSerializer : BaseSerializer
    {
        public override object Deserialize(BinaryTypesReader br, Type type)
        {
            return br.ReadSingle(); //todo: compact

        }

        public override void Serialize(BinaryTypesWriter bw, Type type, object value)
        {
            bw.Write((float)(object)value);
        }
    }

}    
