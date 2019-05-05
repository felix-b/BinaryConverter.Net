using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Serializers
{
    public abstract class BaseInt64Serializer : BaseSerializer
    {

        public override object Deserialize(BinaryTypesReader br, Type type)
        {
            var val = br.Read7BitLong();
            return Convert.ChangeType(val, type);
        }

        public override void Serialize(BinaryTypesWriter bw, Type type, object value)
        {
            var val = Convert.ChangeType(value, typeof(long));
            bw.Write7BitLong((long)val);
        }
    }

    public class Int64Serializer : BaseInt64Serializer
    {
    }

    public class Int32Serializer : BaseInt64Serializer
    {
    }

    public class BooleanSerializer : BaseInt64Serializer
    {
    }

    public class ByteSerializer : BaseInt64Serializer
    {
    }

    public class SByteSerializer : BaseInt64Serializer
    {
    }

    public class Int16Serializer : BaseInt64Serializer
    {
    }

    public class UInt16Serializer : BaseInt64Serializer
    {
    }

    public class UInt32Serializer : BaseInt64Serializer
    {
    }

    public class UInt64Serializer : BaseInt64Serializer
    {
    }

    public class CharSerializer : BaseInt64Serializer
    {
    }
}
