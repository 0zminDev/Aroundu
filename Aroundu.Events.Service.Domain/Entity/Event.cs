namespace Aroundu.Events.Service.Domain.Entity
{
    public class Event
    {
        public Guid PublicKey { get; set; }
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
