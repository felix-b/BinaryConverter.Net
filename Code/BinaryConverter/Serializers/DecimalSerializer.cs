using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Serializers
{
    public class DecimalSerializer : BaseSerializer
    {
        public override object Deserialize(BinaryTypesReader br, Type type)
        {
            return br.ReadCompactDecimal();
        }

        public override void Serialize(BinaryTypesWriter bw, Type type, object value)
        {
            bw.WriteCompactDecimal((decimal)(object)value);
        }
    }

}
