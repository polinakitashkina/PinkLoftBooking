namespace PinkLoftBooking.Api.Models.Domain;

/// <summary>Базовая сущность с идентификатором (наследование для ООП-модели).</summary>
public abstract class DomainEntity
{
    public Guid Id { get; set; }
}
