using System;
using System.Diagnostics;
using Ninject;

namespace Querite
{
    public class QueriteContainer : IDisposable
    {
        private readonly IKernel _kernel;

        public QueriteContainer() : this(new StandardKernel(new NinjectSettings { InjectNonPublic = true }))
        {}

        public QueriteContainer(IKernel kernel)
        {
            _kernel = kernel;
            _kernel.Bind(typeof(ICacheStore<,>)).To(typeof(InMemoryCache<,>)).InSingletonScope();
            _kernel.Bind<CacheToMemoryConvention>().ToSelf().InTransientScope();
        }

        public QueriteContainer Override<TConvention>(TConvention convention) where TConvention : class, IQueriteConvention
        {
            _kernel.Bind<TConvention>().ToMethod(c => convention).InSingletonScope();
            return this;
        }

        public QueriteContainer Add<TModel, TSource>(ICacheStore<IAmQuery<TModel, TSource>, TModel> querycache) where TSource : class
        {
            _kernel.Bind<ICacheStore<IAmQuery<TModel, TSource>, TModel>>().ToMethod(c => querycache).InSingletonScope();
            return this;
        }

        public QueriteContainer Add<TModel, TSource>(ICacheStore<IAmQuery<TModel, TSource>, LastQueryStats> querycache) where TSource : class
        {
            _kernel.Rebind<ICacheStore<IAmQuery<TModel, TSource>, LastQueryStats>>().ToMethod(c => querycache).InSingletonScope();
            return this;
        }

        public IQuery<TSource> Query<TSource>(TSource source) where TSource : class
        {
            return new Query<TSource>(() => DateTime.Now, new CacheStoreFactory(t => _kernel.Get(t)), source);
        }

        public void Dispose()
        {
            _kernel.Dispose();
        }
    }
}