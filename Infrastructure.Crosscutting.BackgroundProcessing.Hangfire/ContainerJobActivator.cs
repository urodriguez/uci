using System;
using Hangfire;
using Unity;

namespace Infrastructure.Crosscutting.BackgroundProcessing.Hangfire
{
    public class ContainerJobActivator : JobActivator
    {
        private readonly IUnityContainer _container;

        public ContainerJobActivator(IUnityContainer container)
        {
            _container = container;
        }

        public override object ActivateJob(Type type)
        {
            return _container.Resolve(type);
        }
    }
}