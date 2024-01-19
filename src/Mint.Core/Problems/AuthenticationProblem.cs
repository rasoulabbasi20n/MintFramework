namespace Mint.Core.Problems;

public class AuthenticationProblem : ApplicationProblem
{
    public AuthenticationProblem(string message) : base(message)
    {
        Status = 401;
        RelatedInfo = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
    }
}
