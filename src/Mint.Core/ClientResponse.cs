using Mint.Core.Problems;

namespace Mint.Core
{
    public record ClientResponse<TResponse> : ProcessResultBase<TResponse>
    {
        private ClientResponse(TResponse result) : base(result) { }
        private ClientResponse(ProblemBase problem) : base(problem) { }

        public static ClientResponse<TResponse> CreateSuccess(TResponse result)
        {
            return new ClientResponse<TResponse>(result);
        }

        public static ClientResponse<TResponse> CreateFailure(ProblemBase problem)
        {
            return new ClientResponse<TResponse>(problem);
        }
    }

    public record ClientResponse : ProcessResultBase
    {
        private ClientResponse() : base() { }
        private ClientResponse(ProblemBase problem) : base(problem) { }

        public static ClientResponse CreateSuccess()
        {
            return new ClientResponse();
        }

        public static ClientResponse CreateFailure(ProblemBase problem)
        {
            return new ClientResponse(problem);
        }
    }
}