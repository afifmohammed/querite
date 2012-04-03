using Ninject.Modules;

namespace querite
{
    public class Querite : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof (IQuery<>)).To(typeof (Query<>)).InRequestScope();
            
            Bind<DecorateQueryByEveryConvention>().ToSelf().InSingletonScope();
            
            Bind<QueryAndLogByConvention>().ToSelf().InSingletonScope();
            Bind<QueryAndCacheByConvention>().ToSelf().InSingletonScope();
            Bind<QueryFromCacheByConvention>().ToSelf().InSingletonScope();
            Bind<EveryQueryConvention>().ToSelf().InSingletonScope();

            Kernel.BindPluggable<ICacheStoreInMemoryConvention>(x => x.InSingletonScope());
            Kernel.BindPluggable<IQueryAndLogConvention>(x => x.InSingletonScope());
            Kernel.BindPluggable<IQueryAndCacheConvention>(x => x.InSingletonScope());
            Kernel.BindPluggable<IQueryFromCacheConvention>(x => x.InSingletonScope());
            
            Kernel.BindPluggable<IQueryConvention>(x => x.InSingletonScope(), x => x.Excluding<EveryQueryConvention>());
            Kernel.BindPluggable<CacheStoreFactory>(x => x.InSingletonScope());

            Kernel.Bind(
                typeof (DecorateQueryBy<>), 
                new ServiceTypeToSelfBindingGenerator(typeof (DecorateQueryBy<>)), 
                x => x.InSingletonScope(), 
                x => x.Excluding<DecorateQueryByEveryConvention>());

            Kernel.Settings.InjectNonPublic = true;
        }
    }
}