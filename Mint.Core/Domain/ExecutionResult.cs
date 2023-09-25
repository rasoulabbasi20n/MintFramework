using Mint.Core.Problems;

namespace Mint.Core.Domain
{
    public class ExecutionResult<TResult>
    {
        public TResult Result { get; set; }
        public bool Success { get; set; }
        public ProblemBase Problem { get; set; }

        private ExecutionResult()
        {
        }

        public static ExecutionResult<TResult> CreateSuccess(TResult result)
        {
            return new ExecutionResult<TResult> { Result = result, Success = true };
        }

        public static ExecutionResult<TResult> CreateFailure(ProblemBase problem)
        {
            return new ExecutionResult<TResult> { Problem = problem, Success = false };
        }
    }

    public class ExecutionResult
    {
        public bool Success { get; set; }
        public ProblemBase Problem { get; set; }

        private ExecutionResult()
        {
        }

        public static ExecutionResult CreateSuccess()
        {
            return new ExecutionResult { Success = true };
        }

        public static ExecutionResult CreateFailure(ProblemBase problem)
        {
            return new ExecutionResult { Problem = problem, Success = false };
        }
    }

}
