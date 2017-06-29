using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EtsyListIt.Utility.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> input)
        {
            return input == null || !input.Any();
        }
    }
}