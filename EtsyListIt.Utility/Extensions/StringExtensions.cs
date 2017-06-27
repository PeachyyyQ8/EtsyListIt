using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

namespace EtsyListIt.Utility.Extensions
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }
        public static string StringValue(this Enum value)
        {
            if (value == null)
            {
                throw new Exception("Unable to get string value from enum.");
            }

            return StringEnum.GetStringValue(value);
        }

        public static string QuickFormat(this string value, object obj1)
        {
            return string.Format(value, obj1);
        }

        public static string QuickFormat(this string value, object obj1, object obj2)
        {
            return string.Format(value, obj1, obj2);
        }
        
        public static string QuickFormat(this string value, object obj1, object obj2, object obj3)
        {
            return string.Format(value, obj1, obj2, obj3);
        }

        public static string QuickFormat(this string value, object[] objects)
        {
            return string.Format(value, objects);
        }

        
    }
}



//public static bool IsValid(this AuthToken _token)
//{
//    if (_token != null)
//    {
//        if (!_token.APIKey.IsNullOrEmpty() && !_token.SharedSecret.IsNullOrEmpty() &&
//            !_token.Key.IsNullOrEmpty() && !_token.AuthTokenSecret.IsNullOrEmpty())
//        {
//            return true;
//        }
//    }
//    return false;
//}