namespace Libraries.Core.Backend.Common
{
    public interface IInit
    {
        void Init();
    }

    public interface IInit<in TArg>
    {
        void Init(TArg arg);
    }

    public interface IInit<in TArgFirst, in TArgSecond>
    {
        void Init(TArgFirst argFirst, TArgSecond argSecond);
    }
}
