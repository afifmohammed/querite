using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Ninject;
using Ninject.Extensions.Conventions;

namespace querite
{
    internal static class Extensions
    {
        /// <summary>
        /// enumerates each item on the <paramref name="items"/> collection and will apply the <paramref name="action"/> on it.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        public static string ExecutingAssmeblyPath(this AppDomain appDomain)
        {
            return string.IsNullOrEmpty(appDomain.RelativeSearchPath)
                       ? appDomain.BaseDirectory
                       : appDomain.RelativeSearchPath;
        }

        public static IKernel BindPluggable<TService>(this IKernel kernel, params Action<AssemblyScanner>[] actions)
        {
            var pluggableService = typeof (TService);
            kernel.Bind(pluggableService, new OverridableBindingGenerator(pluggableService), actions);

            return kernel;
        }

        public static IKernel Bind<TBindingGenerator>(this IKernel kernel, Type service, TBindingGenerator bindingGenerator, params Action<AssemblyScanner>[] actions) where TBindingGenerator : IBindingGenerator
        {
            kernel.Scan(scanner =>
            {
                scanner.FromAssembliesInPath(AppDomain.CurrentDomain.ExecutingAssmeblyPath());
                actions.ForEach(a => a(scanner));
                scanner.Where(target => !target.IsAbstract && !target.IsInterface && target.IsClass);

                if (service.IsGenericType)
                    scanner.Where(target => target.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == service));
                else
                    scanner.WhereTypeInheritsFrom(service);

                scanner.Excluding(service);
                scanner.BindWith(bindingGenerator);
            });

            return kernel;
        }

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