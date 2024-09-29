namespace CourseHub.Core.Entities.Contracts;

/// <summary>
/// The class does not indicate a true "Entity".
/// The app also uses navigation properties for simpler retrieval.
/// 
/// True entities should be referenced by their identifiers.
/// Some "entities" are anemic, some are rich.
/// </summary>
public abstract class Entity : DomainObject
{
    public Guid Id { get; protected set; }
}
