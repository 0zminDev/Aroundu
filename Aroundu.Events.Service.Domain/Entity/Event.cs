namespace Aroundu.Events.Service.Domain.Entity
{
    public class Event
    {
        public int Id { get; set; }
        public Guid PublicKey { get; set; }
        public string Name { get; set; }
    }
}
