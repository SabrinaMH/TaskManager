using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MediatR;

namespace TaskManager.Domain.Infrastructure
{
    public class Mediate
    {
        public IMediator Bootstrap()
        {
            var mediator = new Mediator(SingleInstanceFactory, MultiInstanceFactory);
            return mediator;
        }

        private IEnumerable<object> MultiInstanceFactory(Type serviceType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembliesReferencingMediatr = assemblies.Where(x => x.GetReferencedAssemblies().Any(y => y.Name.Equals("MediatR")));
            IEnumerable<object> types = assembliesReferencingMediatr
                .SelectMany(s => s.ExportedTypes)
                .Where(t => serviceType.GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()))
                .Select(Activator.CreateInstance);
            return types;
        }

        private object SingleInstanceFactory(Type serviceType)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var type = assemblies
                .SelectMany(s => s.ExportedTypes)
                .First(t => serviceType.GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()));
            return Activator.CreateInstance(type);
        }
    }
}