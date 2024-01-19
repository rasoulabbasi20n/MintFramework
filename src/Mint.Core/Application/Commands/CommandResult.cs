using Mint.Core.Application.Events;
using Mint.Core.Domain;
using Mint.Core.Problems;

namespace Mint.Core.Application.Commands
{
    public record CommandResult<TResult> : CommandResult
    {
        public TResult? Result { get; set; }

        private CommandResult()
        {
        }

        public static CommandResult<TResult> CreateSuccess(TResult result, IDomainEvent[] domainEvents)
        {
            return new CommandResult<TResult> { Result = result, RaisedDomainEvents = domainEvents, Success = true };
        }

        public static CommandResult<TResult> CreateFailure(ProblemBase problem)
        {
            return new CommandResult<TResult> { Problem = problem, Success = false };
        }
    }

    public record CommandResult : ProcessResultBase
    {
        public IDomainEvent[] RaisedDomainEvents { get; set; } = Array.Empty<IDomainEvent>();
        public IApplicationEvent[] RaisedApplicationEvents { get; set; } = Array.Empty<IApplicationEvent>();

        protected CommandResult() { }
        protected CommandResult(ProblemBase problem) : base(problem) { }

        public static CommandResult CreateSuccess()
        {
            return new CommandResult();
        }

        public static CommandResult CreateSuccess(IDomainEvent[] domainEvents, IApplicationEvent[] applicationEvents)
        {
            return new CommandResult { RaisedDomainEvents = domainEvents, RaisedApplicationEvents = applicationEvents };
        }

        public static CommandResult CreateFailure(ProblemBase problem)
        {
            return new CommandResult(problem);
        }
    }
}
