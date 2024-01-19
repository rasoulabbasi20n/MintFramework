namespace Mint.Core.Problems;

public abstract class ProblemBase
{
    protected ProblemBase(string message)
    {
        ProblemType = GetType().Name;
        Detail = $"{ProblemType} occurred: {message}";
    }
    public IList<ProblemBase> Problems { get; set; } = new List<ProblemBase>();
    public string ProblemType { get; set; }
    public string RelatedInfo { get; set; }
    public string Title { get; set; }
    public string Detail { get; set; }

    public void AddProblem(ProblemBase problem)
    {
        Problems.Add(problem);
    }

}
