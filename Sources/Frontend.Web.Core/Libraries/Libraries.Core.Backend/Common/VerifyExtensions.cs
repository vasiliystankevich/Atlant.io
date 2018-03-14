using System;

namespace Libraries.Core.Backend.Common
{
    public static class VerifyExtensions
    {
        public static void IsNull(this object sender, string message)
        {
            sender.Verify(() => sender == null, message);
        }

        public static void IsNotNull(this object sender, Action action)
        {
            if (sender != null) action();
        }

        public static void Verify(this object sender, Func<bool> verifyFunctor, string message)
        {
            if (verifyFunctor()) throw new Exception(message);
        }

        public static void Verify<TVerifyObject>(this object sender, TVerifyObject verifyObject, Func<TVerifyObject, bool> verifyFunctor, string message)
        {
            if (verifyFunctor(verifyObject)) throw new Exception(message);
        }
        public static void Verify<TVerifyObjectFirst, TVerifyObjectSecond>(this object sender, TVerifyObjectFirst verifyObjectFirst, TVerifyObjectSecond verifyObjectSecond, Func<TVerifyObjectFirst, TVerifyObjectSecond, bool> verifyFunctor, string message)
        {
            if (verifyFunctor(verifyObjectFirst, verifyObjectSecond)) throw new Exception(message);
        }
    }
}
