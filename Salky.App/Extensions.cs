namespace Salky.App
{
    public static class Extensions
    {
        public static T ThrowIfNull<T>(this T? @object, string msg)
        {
            if (@object == null)
                throw new NullReferenceException(msg);
            return @object;
        }

        public static T ThrowIfTrue<T>(this T @object, Func<T,bool> condition, string? msg = null)
        {
            if (condition(@object))
                throw new Exception(msg??"");
            return @object;
        }


        public static T ThrowIfTrue<T>(this T @object, Func<T, Task<bool>> condition, string? msg = null)
        {
            if (condition(@object).Result)
                throw new Exception(msg ?? "");
            return @object;
        }



        public static T ThrowIfNull<T>(this T? @object)
        {
            return @object.ThrowIfNull($"{typeof(T).Name} cannot be null");
        }


    }
}
