using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace querite
{
    public static class Extensions
    {
        public static TValue TryToList<TValue>(this TValue value)
        {
            return value;
        }

        public static string EnglishName(this Type type)
        {
            var name = type.Name;

            if (type.IsGenericType)
            {
                name += "Of";
                name = type.GetGenericArguments().Aggregate(name, (current, arg) => current + "{0}And".For(arg.EnglishName()));
            }

            name = name.Replace("`", "").Replace("1", "").Replace("2", "").Replace("3", "");

            const string tobeRemoved = "And";

            if (name.EndsWith(tobeRemoved))
                name = name.Remove(name.Length - tobeRemoved.Length, tobeRemoved.Length);

            return name;
        }

        /// <summary>
        /// extension method to invoke <see cref="string.Format(string,object[])"/>
        /// </summary>
        public static string For(this string input, params object[] args)
        {
            return string.Format(input, args);
        }

        public static string ToMd5Fingerprint(this string s)
        {
            var bytes = Encoding.Unicode.GetBytes(s.ToCharArray());
            var hash = new MD5CryptoServiceProvider().ComputeHash(bytes);

            return hash.Aggregate(new StringBuilder(32),
                (sb, b) => sb.Append(b.ToString("X2")))
                .ToString();
        }
    }
}