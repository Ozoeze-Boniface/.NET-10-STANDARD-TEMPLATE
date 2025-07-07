namespace CityCode.MandateSystem.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

public abstract class BaseEntity
{
    // This can easily be modified to be BaseEntity<T> and public T Id to support different key types.
    // Using non-generic integer types for simplicity
    //public int Id { get; set; }

    private readonly List<BaseEvent> domainEvents = [];

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => this.domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent) => this.domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(BaseEvent domainEvent) => this.domainEvents.Remove(domainEvent);

    public void ClearDomainEvents() => this.domainEvents.Clear();
}
