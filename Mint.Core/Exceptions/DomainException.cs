namespace Mint.Core.Gaurds
{
    public class DomainException : Exception
    {
        public DomainException(string? message) : base($"Domain exception occured: '{message}'")
        {
        }
    }
}
