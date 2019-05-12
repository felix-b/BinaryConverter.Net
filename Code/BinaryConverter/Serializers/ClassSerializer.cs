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
            var classMap = SerializerRegistry.GetClassMap(type); //todo: also base types?

            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .OrderBy(x => x.MetadataToken);

            var instance = Activator.CreateInstance(type);


            foreach (var prop in props)
            {
                MemberMap memberMap = classMap?.GetMemberMap(prop.Name);
                if (memberMap?.Ignored == true) continue;

                prop.SetValue(instance, Serializer.DeserializeObject(br, prop.PropertyType, settings, memberMap?.Serializer, memberMap?.SerializerArg));
            }

            return instance;
        }

        public override void Serialize(BinaryTypesWriter bw, Type type, SerializerSettings settings, ISerializerArg serializerArg, object value)
        {
            var classMap = SerializerRegistry.GetClassMap(type); //todo: also base types?

            var props = value.GetType().GetProperties()
                .OrderBy(x => x.MetadataToken)
                .ToList();

            foreach (var prop in props)
            {
                MemberMap memberMap = classMap?.GetMemberMap(prop.Name);
                if (memberMap?.Ignored == true) continue;

                var val = prop.GetValue(value);
                Serializer.SerializeObject(prop.PropertyType, val, bw, settings, memberMap?.Serializer, memberMap?.SerializerArg);
            }
        }
    }
}
