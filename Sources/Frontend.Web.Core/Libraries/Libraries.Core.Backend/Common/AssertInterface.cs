namespace Libraries.Core.Backend.Common
{
    public interface IVoidAssert<TName, in TArgFirst>
    {
        void Assert(TArgFirst argFirst);
    }
    public interface IVoidAssert<TName, in TArgFirst, in TArgSecond>
    {
        void Assert(TArgFirst argFirst, TArgSecond argSecond);
    }

    public interface IAssert<TName, in TArgFirst, out TResult>
    {
        TResult Assert(TArgFirst argFirst);
    }
    public interface IAssert<TName, in TArgFirst, in TArgSecond, out TResult>
    {
        TResult Assert(TArgFirst argFirst, TArgSecond argSecond);
    }

    public static class AssertExtensions
    {
        public static void Assert<TName, TArgFirst>(this IVoidAssert<TName, TArgFirst> sender, TArgFirst argFirst)
        {
            sender.Assert(argFirst);
        }
        public static void Assert<TName, TArgFirst, TArgSecond>(this IVoidAssert<TName, TArgFirst, TArgSecond> sender, TArgFirst argFirst, TArgSecond argSecond)
        {
            sender.Assert(argFirst, argSecond);
        }
        public static TResult Assert<TName, TArgFirst, TResult>(this IAssert<TName, TArgFirst, TResult> sender, TArgFirst argFirst)
        {
            return sender.Assert(argFirst);
        }

        public static TResult Assert<TName, TArgFirst, TArgSecond, TResult>(this IAssert<TName, TArgFirst, TArgSecond, TResult> sender, TArgFirst argFirst, TArgSecond argSecond)
        {
            return sender.Assert(argFirst, argSecond);
        }
    }
}
