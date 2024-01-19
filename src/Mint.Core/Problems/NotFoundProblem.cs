namespace Mint.Core.Problems;

public class NotFoundProblem : ApplicationProblem
{
    public NotFoundProblem(string message) : base(message)
    {
        Status = 404;
        RelatedInfo = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
        Title = "The specified resource was not found.";
    }
}
