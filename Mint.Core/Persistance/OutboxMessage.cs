using Mint.Core.Domain;

namespace Mint.Core.Persistance
{
    public class OutboxMessage : AggregateRoot<string>
    {
        public string Id { get; set; }
        public bool Dispatched { get; set; }
        public DateTime DispatchedAt { get; set; }
        public DateTime PublishedDate { get; set; }
        public string  Data { get; set; }  
        
        public void Dispatch()
        {
            Dispatched = true;
            DispatchedAt= DateTime.Now;
        }
    }
}
