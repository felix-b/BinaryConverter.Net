using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Serializers
{
    public class FloatingPointSerializerArg : ISerializerArg
    {
        public int DecimalDigits { get; set; } = -1;
    }

    class DoubleSerializer : BaseSerializer
    {
        public override object Deserialize(BinaryTypesReader br, Type type, SerializerSettings settings, ISerializerArg serializerArg)
        {
            var typedSerializerArg = GetSerializerArg<FloatingPointSerializerArg>(type, settings, serializerArg);

            if (typedSerializerArg != null)
            {
                if (typedSerializerArg.DecimalDigits >= 0)
                {
                    var decValue = br.ReadCompactDecimal(typedSerializerArg.DecimalDigits);
                    return (double)decValue;
                }
            }

            return br.ReadDouble();
        }

        public override void Serialize(BinaryTypesWriter bw, Type type, SerializerSettings settings, ISerializerArg serializerArg, object value)
        {
            var typedSerializerArg = GetSerializerArg<FloatingPointSerializerArg>(type, settings, serializerArg);

            if (typedSerializerArg != null)
            {
                if (typedSerializerArg.DecimalDigits >= 0)
                {
                    bw.WriteCompactDecimal(Convert.ToDecimal(value), typedSerializerArg.DecimalDigits);
                    return;
                }
            }

            bw.Write((double)value);
        }
    }

    class SingleSerializer : BaseSerializer
    {
        public override object Deserialize(BinaryTypesReader br, Type type, SerializerSettings settings, ISerializerArg serializerArg)
        {
            var typedSerializerArg = GetSerializerArg<FloatingPointSerializerArg>(type, settings, serializerArg);

            if (typedSerializerArg != null)
            {
                if (typedSerializerArg.DecimalDigits >= 0)
                {
                    var decValue = br.ReadCompactDecimal(typedSerializerArg.DecimalDigits);
                    return (float)decValue;
                }
            }

            return br.ReadSingle();

        }

        public override void Serialize(BinaryTypesWriter bw, Type type, SerializerSettings settings, ISerializerArg serializerArg, object value)
        {
            var typedSerializerArg = GetSerializerArg<FloatingPointSerializerArg>(type, settings, serializerArg);

            if (typedSerializerArg != null)
            {
                if (typedSerializerArg.DecimalDigits >= 0)
                {
                    bw.WriteCompactDecimal(Convert.ToDecimal(value), typedSerializerArg.DecimalDigits);
                    return;
                }
            }

            bw.Write((float)(object)value);
        }
    }

}
