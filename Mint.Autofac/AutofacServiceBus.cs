using Autofac;
using Mint.Core;
using Mint.Core.Application;
using Mint.Core.Persistance;

namespace Mint.Autofac
{
    public class AutofacServiceBus : ServiceBusBase
    {
        private readonly ILifetimeScope _scope;

        public AutofacServiceBus(
            ILifetimeScope scope,
            ILoggingService logger,
            IUnitOfWork unitOfWork,
            bool enableOutbox)
            : base(logger, unitOfWork, enableOutbox)
        {
            _scope = scope;
        }

        protected override TService ResolveService<TService>()
        {
            return _scope.Resolve<TService>();
        }

        protected override IEnumerable<TService> ResolveServices<TService>()
        {
            return _scope.Resolve<IEnumerable<TService>>();
        }
    }
}