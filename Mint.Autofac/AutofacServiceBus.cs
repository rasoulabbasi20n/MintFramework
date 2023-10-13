using Autofac;
using Mint.Core;
using Mint.Core.Application;
using Mint.Core.Application.Events;
using Mint.Core.Persistance;
using Mint.Core.Transport;

namespace Mint.Autofac
{
    public class AutofacServiceBus : ServiceBusBase
    {
        private readonly ILifetimeScope _scope;

        public AutofacServiceBus(
            ILifetimeScope scope,
            ILoggingService logger,
            IUnitOfWork unitOfWork,
            IMessageTransporter messageTransporter)
            : base(logger, unitOfWork, messageTransporter)
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