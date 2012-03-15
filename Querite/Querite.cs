using Ninject.Modules;

namespace querite
{
    public class Querite : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof (IQuery<>)).To(typeof (Query<>)).InRequestScope();
            Kernel.Settings.InjectNonPublic = true;
        }
    }
}