using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Aroundu.Events.Service.Infrastructure.EFCore
{
    public class EventsDbContext : DbContext
    {
        public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options) { }

        public DbSet<Aroundu.Events.Service.Domain.Entity.Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aroundu.Events.Service.Domain.Entity.Event>().HasKey(e => e.Id);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}
