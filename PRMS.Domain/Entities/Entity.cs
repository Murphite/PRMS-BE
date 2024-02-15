namespace PRMS.Domain.Entities;

public abstract class Entity
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
}