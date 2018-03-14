using static System.Activator;

namespace Libraries.Core.Backend.Common
{
    public class Creator<T> where T:class, new() 
    {
        public static T Create() => new T();

        public static T Create<TArg1>(TArg1 arg1) => (T)CreateInstance(typeof(T), arg1);

        public static T Create<TArg1, TArg2>(TArg1 arg1, TArg2 arg2) => (T) CreateInstance(typeof(T), arg1, arg2);
    }
}
