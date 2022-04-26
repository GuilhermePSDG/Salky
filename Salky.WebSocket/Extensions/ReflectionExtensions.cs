using System.Reflection;



namespace Salky.WebSocket.Extensions
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="AtributeType"></typeparam>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="Exception"></exception>
        public static AtributeType GetRequiredAtribute<AtributeType>(this MemberInfo memberInfo) where AtributeType : Attribute
        {
            var res = memberInfo.GetCustomAttribute(typeof(AtributeType));
            if (res == null) throw new NullReferenceException($"{memberInfo.Name} do not has the cumtom atribute {typeof(AtributeType).FullName}");
            return (AtributeType)res ?? throw new Exception($"Cannot cast {memberInfo.Name} into {typeof(AtributeType).Name}  ");
        }
        /// <summary>
        /// Return all methods where Method contains atribute of parameter Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="class"></param>
        /// <returns></returns>
        public static MethodInfo[] GetMethodsWithAtribute<T>(this Type @class)
        {
            return
                @class.GetMethods()
                .Where(f => f.GetCustomAttribute(typeof(T)) != null)
                .ToArray();
        }
        public static object? TryCreateInstance(this Type type)
        {
            try
            {
                return Activator.CreateInstance(type);
            }
            catch
            {
                return null;
            }
        }
    }

}
