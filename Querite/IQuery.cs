using System;
using System.Collections.Generic;
using Ninject;
using Ninject.Modules;

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

    public interface IQuery<out TSource>
    {
        IEnumerable<TModel> Execute<TModel>(IAmQuery<TModel, TSource> query);
        IQuery<TSource> Count(Action<int> count);
    }

    internal class Query<TSource> : IQuery<TSource>
    {
        protected Action<int> SetCount;
        [Inject] protected TSource Source { get; set; }

        public IEnumerable<TModel> Execute<TModel>(IAmQuery<TModel, TSource> query)
        {
            return query.Count(SetCount).Apply(Source);
        }

        public IQuery<TSource> Count(Action<int> count)
        {
            SetCount = count;
            return this;
        }
    }

    public class Querite : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof (IQuery<>)).To(typeof (Query<>)).InRequestScope();
            Kernel.Settings.InjectNonPublic = true;
        }
    }
}