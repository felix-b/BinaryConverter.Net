using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Serializers
{
    public class DateTimeSerializer : BaseSerializer
    {
        public override object Deserialize(BinaryTypesReader br, Type type)
        {
            return new DateTime(br.Read7BitLong(), DateTimeKind.Utc); //todo: compact
        }

        public override void Serialize(BinaryTypesWriter bw, Type type, object value)
        {
            bw.Write7BitLong(((DateTime)(object)value).Ticks); //todo: compact
        }
    }
}
