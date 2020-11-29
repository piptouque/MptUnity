
namespace MptUnity.Utility
{
    using T = System.Single;
    static class ApproxFloat
    {
        const int c_ulpDefault = 2;

        public static bool AreApproximatelyEqual(T a, T b, int ulp = c_ulpDefault)
        {
            // see example:
            // https://en.cppreference.com/w/cpp/types/numeric_limits/epsilon
            T dist = System.Math.Abs(b - a);
            return dist <= GetErrorMargin(a, b, ulp) || dist < GetMin();
        }

        static T GetErrorMargin(T a, T b, int ulp)
        {
            return GetEpsilon() * System.Math.Abs(b - a) * ulp;
        }

        static T GetEpsilon()
        {
            return T.Epsilon;
        }

        static T GetMin()
        {
            return T.MinValue;
        }
    }
}