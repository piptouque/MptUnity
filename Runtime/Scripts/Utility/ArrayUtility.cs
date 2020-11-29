
using System.Linq;

namespace MptUnity.Utility
{
    static class ArrayUtility
    {
        // No Fill method for arrays before .NET 5.0
        // We are running with .NET 4.X
        public static void Fill<T>(this T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; ++i)
            {
                arr[i] = value;
            }
        }

        /// <summary>
        /// get the index of the maximum value in arr.
        /// </summary>
        /// <param name="arr"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int GetIndexMax<T>(this T[] arr)
        {
            return System.Array.FindIndex(arr, el => el.Equals(arr.Max()));
        }
    }
}