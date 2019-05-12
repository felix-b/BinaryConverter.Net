using BinaryConverter.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BinaryConverter
{
    public class MemberMap
    {
        public bool Ignored { get;  set; }
        public ISerializer Serializer { get; set; }
        public ISerializerArg SerializerArg { get; set; }
    }

    public interface IClassMap
    {
        MemberMap GetMemberMap(string propertyName);
    }

    public class ClassMap<TClass> : IClassMap
    {
        private Dictionary<string, MemberMap> _memberMaps = new Dictionary<string, MemberMap>();

        public ClassMap()
        {
        }

        public ClassMap(Action<ClassMap<TClass>> classMapInitializer)
        {
            classMapInitializer(this);
        }

        public MemberMap GetMemberMap(string propertyName)
        {
            _memberMaps.TryGetValue(propertyName, out var memberMap);
            return memberMap;
        }

        public MemberMap MapProperty<TMember>(Expression<Func<TClass, TMember>> propertyLambda)
        {
            var memberName = GetMemberInfoFromLambda(propertyLambda).Name;
            return MapProperty<TMember>(memberName);
        }

        public MemberMap MapProperty<TMember>(string propertyName)
        {
            if (_memberMaps.TryGetValue(propertyName, out var memberMap) == false)
            {
                memberMap = new MemberMap();
                _memberMaps[propertyName] = memberMap;
            }

            return memberMap;
        }

        #region from MongoDB.Bson.Serialization.BsonClassMap
        //============================================================================================
        //Note: 
        // copied from MongoDB.Bson.Serialization.BsonClassMap

        private static MemberInfo GetMemberInfoFromLambda<TMember>(Expression<Func<TClass, TMember>> memberLambda)
        {
            var body = memberLambda.Body;
            MemberExpression memberExpression;
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    memberExpression = (MemberExpression)body;
                    break;
                case ExpressionType.Convert:
                    var convertExpression = (UnaryExpression)body;
                    memberExpression = (MemberExpression)convertExpression.Operand;
                    break;
                default:
                    throw new Exception("Invalid lambda expression");
            }
            var memberInfo = memberExpression.Member;
            if (memberInfo is PropertyInfo)
            {
                if (memberInfo.DeclaringType.GetTypeInfo().IsInterface)
                {
                    memberInfo = FindPropertyImplementation((PropertyInfo)memberInfo, typeof(TClass));
                }
            }
            else if (!(memberInfo is FieldInfo))
            {
                throw new Exception("Invalid lambda expression");
            }
            return memberInfo;
        }

        private static PropertyInfo FindPropertyImplementation(PropertyInfo interfacePropertyInfo, Type actualType)
        {
            var interfaceType = interfacePropertyInfo.DeclaringType;

            var interfaceMap = actualType.GetInterfaceMap(interfaceType);

            var interfacePropertyAccessors = interfacePropertyInfo.GetAccessors(true);

            var actualPropertyAccessors = interfacePropertyAccessors.Select(interfacePropertyAccessor =>
            {
                var index = Array.IndexOf<MethodInfo>(interfaceMap.InterfaceMethods, interfacePropertyAccessor);

                return interfaceMap.TargetMethods[index];
            });

            return actualType.GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Single(propertyInfo =>
                {
                    var propertyAccessors = propertyInfo.GetAccessors(true);
                    return actualPropertyAccessors.All(x => propertyAccessors.Contains(x));
                });
        }

        //============================================================================================
        //============================================================================================
        #endregion

    }
}
