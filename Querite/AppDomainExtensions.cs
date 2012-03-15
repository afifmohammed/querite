using System;

namespace querite
{
    internal static class AppDomainExtensions
    {
        public static string ExecutingAssmeblyPath(this AppDomain appDomain)
        {
            return string.IsNullOrEmpty(appDomain.RelativeSearchPath)
                       ? appDomain.BaseDirectory
                       : appDomain.RelativeSearchPath;
        }
    }
}