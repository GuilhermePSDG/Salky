using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSocket
{
    class ObjectMapper
    {
        public ObjectMapper() 
        {
            throw new NotImplementedException();
        }
        public object MapProperties(Type type)
        {
            mappedTypes = new();
            return SafeMapPropertiesLoopingPrevent(type) ?? throw new NullReferenceException();
        }
        private Dictionary<string, Type> mappedTypes = new();
        private object? SafeMapPropertiesLoopingPrevent(Type? type, MemberInfo? HASMEMBER = null)
        {
            if (type == null) return null;
            if (IsPrimitive(type)) return new SimpleType(HASMEMBER == null ? type.Name : HASMEMBER.Name, type.Name);

            mappedTypes.Add(type.Name, type);
            var result = new Dictionary<string, List<object?>>();
            result[type.Name] = new List<object?>();

            foreach (var member in type.GetProperties())
            {
                if (IsPrimitive(member.PropertyType))
                {
                    result[type.Name].Add(new SimpleType(member.Name, member.PropertyType.Name));
                }
                else if (IsCollection(member.PropertyType))
                {
                    var arrayPropertyType = member.PropertyType.GetElementType() ?? throw new NullReferenceException();
                    if (mappedTypes.ContainsKey(arrayPropertyType.Name))
                        result[type.Name].Add(new SimpleType(member.Name, member.PropertyType.Name));
                    else
                        result[type.Name].Add(new ArrayType(member.Name, arrayPropertyType.Name, new ComplexType(arrayPropertyType.Name, SafeMapPropertiesLoopingPrevent(arrayPropertyType, member) ?? throw new NullReferenceException())));
                }
                else
                {
                    if (mappedTypes.ContainsKey(member.PropertyType.Name))
                        result[type.Name].Add(new SimpleType(member.Name, member.PropertyType.Name));
                    else
                        result[type.Name].Add(SafeMapPropertiesLoopingPrevent(member.PropertyType));
                }
            }
            return result;
        }

        bool IsPrimitive(Type type)
        {
            if (type.IsPrimitive)
                return true;
            if (type.Equals(typeof(string)))
                return true;
            if (type.Equals(typeof(DateTime)))
                return true;
            return false;
        }
        bool IsCollection(Type type)
        {
            if (type.IsArray)
                return true;
            if (type.GetInterface(nameof(ICollection)) != null)
                return true;
            return false;
        }

        record SimpleType(string Name, string TypeName);
        record ArrayType(string Name, string TypeName, ComplexType ArrayObject);
        record ComplexType(string Name, object ComplexObject);
    }

}
