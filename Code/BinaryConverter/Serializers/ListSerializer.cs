using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter.Serializers
{
    class ListSerializer : BaseSerializer
    {
        public override object Deserialize(BinaryTypesReader br, Type type)
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
                instance.Add(Deserializer.DeserializeObject(br, genericArgtype));
            }
            return instance;
        }

        public override void Serialize(BinaryTypesWriter bw, Type type, object value)
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
                Serializer.SerializeObject(genericArgtype, list[i], bw);
            }
        }
    }
}
