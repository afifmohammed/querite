using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Querite
{
    internal static class QueriteInternalExtensions
    {
        public static TValue TryToList<TValue>(this TValue value)
        {
            return value;
        }

        public static string ToMd5Fingerprint(this string s)
        {
            var bytes = Encoding.Unicode.GetBytes(s.ToCharArray());
            var hash = new MD5CryptoServiceProvider().ComputeHash(bytes);

            return hash.Aggregate(new StringBuilder(32),
                (sb, b) => sb.Append(b.ToString("X2")))
                .ToString();
        }

        public static bool IsNull(this object instance)
        {
            return ReferenceEquals(instance, null);
        }

        public static bool IsNotNull(this object instance)
        {
            return !instance.IsNull();
        }
    }
}