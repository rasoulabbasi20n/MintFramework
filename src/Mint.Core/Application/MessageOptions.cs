namespace Mint.Core.Application
{
    public class MessageOptions<TMessage>
    {
        public string? Destination { get; set; }
        public string GetDestination()
        {
            return Destination ?? typeof(TMessage).FullName;
        }
    }
}
