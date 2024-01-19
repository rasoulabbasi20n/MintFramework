using Microsoft.EntityFrameworkCore;
using Mint.Core.Persistance;

namespace Mint.EntityFramework
{
    public class EfDbContextWithOutbox : DbContext
    {
        protected DbSet<OutboxMessage> OutboxMessages { get; set; }
    }
}
