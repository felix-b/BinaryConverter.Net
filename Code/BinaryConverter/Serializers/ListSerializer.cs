using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Serializers
{
    class ListSerializer : BaseSerializer
    {
        public override bool CommonNullHandle { get { return false; } }

        public override object Deserialize(BinaryTypesReader br, Type type, SerializerSettings settings, ISerializerArg serializerArg)
        {
            int count = br.Read7BitInt();
            if (count == -1)
            {
                return null;
            }
            var instance = (IList)Activator.CreateInstance(type);
            Type genericArgtype = type.GetGenericArguments()[0];

            for (int i = 0; i < count; i++)
            {
                instance.Add(Serializer.DeserializeObject(br, genericArgtype, settings, null, null));
            }
            return instance;
        }

        public override void Serialize(BinaryTypesWriter bw, Type type, SerializerSettings settings, ISerializerArg serializerArg, object value)
        {
            IList list = value as IList;
            if (list == null)
            {
                bw.Write7BitLong(-1);
                return;
            }

            var count = list.Count;
            bw.Write7BitLong(count);
            Type genericArgtype = type.GetGenericArguments()[0];
            for (int i = 0; i < count; i++)
            {
                Serializer.SerializeObject(genericArgtype, list[i], bw, settings, null, null);
            }
        }
    }
}
