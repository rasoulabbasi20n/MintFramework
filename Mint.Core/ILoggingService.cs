using Mint.Core.Problems;

namespace Mint.Core
{
    public interface ILoggingService
    {
        void Info(string message);
        void Error(ProblemBase problem);
        void Error(Exception exception);
        void Error(string message);
    }
}
