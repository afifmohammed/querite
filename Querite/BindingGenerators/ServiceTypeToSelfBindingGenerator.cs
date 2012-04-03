using System;
using Ninject;
using Ninject.Activation;
using Ninject.Extensions.Conventions;

namespace querite
{
    internal class ServiceTypeToSelfBindingGenerator : IBindingGenerator
    {
        private readonly Type _service;

        public ServiceTypeToSelfBindingGenerator(Type service)
        {
            _service = service;
        }

        public void Process(Type type, Func<IContext, object> scopeCallback, IKernel kernel)
        {
            if (type.IsInterface || type.IsAbstract) return;

            if (!_service.IsAssignableFrom(type)) return;

            kernel.Bind(type).ToSelf().InScope(scopeCallback);
        }
    }
}