using System.Reflection;



namespace Salky.WebSocket.Extensions
{
    public static class ReflectionExtensions
    {
        public static Type[] GetAllTypesInCurrentAssembly<T>()
        {
            return AppDomain
            .CurrentDomain
            .GetAssemblies()
            .Where(f => f.GetName().Name == AppDomain.CurrentDomain.FriendlyName)
            .SelectMany(f => f.GetTypes())
            .Where(x =>x.IsAssignableTo(typeof(T)))
            .ToArray();
        }

        public static Type[] GetAllTypes<T>()
        {
            return AppDomain
            .CurrentDomain
            .GetAssemblies()
            .SelectMany(f => f.GetTypes())
            .Where(x => x.IsAssignableTo(typeof(T)))
            .ToArray();
        }


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
            if (res == null) throw new NullReferenceException($"{memberInfo.Name} do not has the custom atribute {typeof(AtributeType).FullName}");
            return (AtributeType)res ?? throw new Exception($"Cannot cast {memberInfo.Name} into {typeof(AtributeType).Name}");
        }
        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="class"></param>
        /// <returns>All <see cref="MethodInfo"/> of a <see langword="class"/> where <see cref="MethodInfo"/> contains a <see cref="Attribute"/> of type <typeparamref name="T"/> </returns>
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
