using System;
using Ninject;

namespace Querite
{
    public class QueriteContainer : IDisposable
    {
        private readonly IKernel _kernel;
        private delegate Func<DateTime> GetTime();
        private readonly bool _iOwnTheKernel;

        public QueriteContainer() : this(new StandardKernel(new NinjectSettings { InjectNonPublic = true }))
        {
            _iOwnTheKernel = true;
        }

        public QueriteContainer(IKernel kernel)
        {
            _kernel = kernel;
            _kernel.Bind(typeof(ICacheStore<,>)).To(typeof(InMemoryCache<,>)).InSingletonScope();
            _kernel.Bind<CacheToMemoryConvention>().ToSelf();
            _kernel.Bind<GetTime>().ToMethod(c => () => () => DateTime.Now);
            _kernel.Bind<CacheStoreFactory>().ToMethod(c => new CacheStoreFactory(t => _kernel.Get(t)));
        }
        
        public QueriteContainer Override<TConvention>(TConvention convention) where TConvention : class, IQueriteConvention
        {
            _kernel.Rebind<TConvention>().ToMethod(c => convention).InSingletonScope();
            return this;
        }

        public QueriteContainer Add<TModel, TSource>(ICacheStore<IAmQuery<TModel, TSource>, TModel> queryCache) where TSource : class
        {
            _kernel.Bind<ICacheStore<IAmQuery<TModel, TSource>, TModel>>()
                   .ToMethod(c => queryCache)
                   .InSingletonScope();

            return this;
        }

        public QueriteContainer Add<TModel, TSource>(ICacheStore<IAmQuery<TModel, TSource>, LastQueryStats> queryStatsCache) where TSource : class
        {
            _kernel.Bind<ICacheStore<IAmQuery<TModel, TSource>, LastQueryStats>>()
                   .ToMethod(c => queryStatsCache)
                   .InSingletonScope();

            return this;
        }

        public QueriteContainer Override(Func<DateTime> getTime)
        {
            _kernel.Rebind<GetTime>().ToMethod(c => () => getTime);
            return this;
        }

        public IQuery<TSource> Query<TSource>(TSource source) where TSource : class
        {
            return new Query<TSource>(_kernel.Get<GetTime>()(), _kernel.Get<CacheStoreFactory>(), () => source);
        }
        
        public void Dispose()
        {
            if(_iOwnTheKernel) _kernel.Dispose();
        }
    }
}