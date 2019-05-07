using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BinaryConverter.Serializers
{
    class ClassSerializer : BaseSerializer
    {
        public override object Deserialize(BinaryTypesReader br, Type type, SerializerSettings settings, ISerializerArg serializerArg)
        {
            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .OrderBy(x => x.MetadataToken);

            var instance = Activator.CreateInstance(type);


            foreach (var prop in props)
            {
                prop.SetValue(instance, Serializer.DeserializeObject(br, prop.PropertyType, settings, null));
            }

            return instance;
        }

        public override void Serialize(BinaryTypesWriter bw, Type type, SerializerSettings settings, ISerializerArg serializerArg, object value)
        {
            var props = value.GetType().GetProperties()
                .OrderBy(x => x.MetadataToken)
                .ToList();

            foreach (var prop in props)
            {
                var val = prop.GetValue(value);
                Serializer.SerializeObject(prop.PropertyType, val, bw, settings, null);
            }
        }
    }
}
