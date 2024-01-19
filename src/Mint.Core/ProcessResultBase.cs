using Mint.Core.Problems;

namespace Mint.Core
{
    public abstract record ProcessResultBase<TResult> : ProcessResultBase
    {
        public TResult? Result { get; set; }

        protected ProcessResultBase(TResult result)
        {
            Result = result;
        }

        protected ProcessResultBase(ProblemBase problem) : base(problem)
        {
        }
    }

    public abstract record ProcessResultBase
    {
        public bool Success { get; set; }
        public ProblemBase? Problem { get; set; }

        protected ProcessResultBase()
        {
            Success = true;
        }

        protected ProcessResultBase(ProblemBase problem)
        {
            Problem = problem;
            Success = false;
        }
    }
}
