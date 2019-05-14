using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Serializers
{
    public class DecimalSerializer : BaseSerializer
    {
        public override object Deserialize(BinaryTypesReader br, Type type, SerializerSettings settings, ISerializerArg serializerArg)
        {
            var typedSerializerArg = GetSerializerArg<FloatingPointSerializerArg>(type, settings, serializerArg);

            if (typedSerializerArg != null)
            {
                return br.ReadCompactDecimal(typedSerializerArg.DecimalDigits);
            }

            return br.ReadCompactDecimal();
        }

        public override void Serialize(BinaryTypesWriter bw, Type type, SerializerSettings settings, ISerializerArg serializerArg, object value)
        {
            var typedSerializerArg = GetSerializerArg<FloatingPointSerializerArg>(type, settings, serializerArg);

            if (typedSerializerArg != null)
            {
                bw.WriteCompactDecimal((decimal)(object)value, typedSerializerArg.DecimalDigits);
                return;
            }

            bw.WriteCompactDecimal((decimal)(object)value);
        }
    }

}
