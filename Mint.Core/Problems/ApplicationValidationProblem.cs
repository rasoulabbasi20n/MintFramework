namespace Mint.Core.Problems;

public class ApplicationValidationProblem : ApplicationProblem
{
    public ApplicationValidationProblem(string message)
        : base(message)
    {
        Status = 400;
        RelatedInfo = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
    }
}
