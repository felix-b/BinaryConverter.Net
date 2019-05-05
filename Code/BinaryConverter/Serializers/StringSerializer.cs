using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Serializers
{
    public class StringSerializer : BaseSerializer
    {

        public override object Deserialize(BinaryTypesReader br, Type type)
        {
            return br.ReadString();
        }

        public override void Serialize(BinaryTypesWriter bw, Type type, object value)
        {
            bw.Write((string)(object)value);
        }
    }
}
