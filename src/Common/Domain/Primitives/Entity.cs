namespace Domain.Primitives;

/// <summary>
/// Base abstract class for all domain entities.
/// Provides a unique identifier and equality implementation based on it.
/// </summary>
/// <typeparam name="TEntityId">The type of the entity identifier.</typeparam>
public abstract class Entity<TEntityId> : IEquatable<Entity<TEntityId>>, IEntity where TEntityId : class, IEntityId
{

    /// <summary>
    /// Initializes a new entity with a specific identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity. Cannot be null.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="id"/> is null.</exception>
    protected Entity(TEntityId id) : this()
    {
        Id = id ?? throw new ArgumentException("The entity identifier is required.", nameof(id));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TEntityId}"/> class.
    /// </summary>
    /// <remarks>
    /// Required for deserialization.
    /// </remarks>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    protected Entity(){}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    public TEntityId Id { get; private init; }

    /// <summary>
    /// Compares two entities for equality based on reference or identifier.
    /// </summary>
    public static bool operator ==(Entity<TEntityId>? a, Entity<TEntityId>? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    /// <summary>
    /// Compares two entities for inequality.
    /// </summary>
    public static bool operator !=(Entity<TEntityId>? a, Entity<TEntityId>? b) => !(a == b);

    /// <inheritdoc />
    public virtual bool Equals(Entity<TEntityId>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.GetType() != GetType())
        {
            return false;
        }

        return ReferenceEquals(this, other) || Id == other.Id;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not Entity<TEntityId> other)
        {
            return false;
        }

        return Id == other.Id;
    }

    /// <inheritdoc />
    public override int GetHashCode() => Id.GetHashCode();
}
