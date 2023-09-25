namespace Mint.Core.Application.Events
{
    public interface IApplicationEvent
    {
        public string Name { get; }
        public Guid Id { get; }
        public string UserId { get; }
        public DateTime PublishedAt => DateTime.Now;
    }
}
