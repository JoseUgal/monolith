using Domain.Primitives;

namespace Modules.Workspaces.Domain.Workspaces;

public sealed class Workspace : Entity<WorkspaceId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Workspace"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="name">The name.</param>
    private Workspace(WorkspaceId id, WorkspaceName name) : base(id)
    {
        Name = name;
    }
    
    /// <summary>
    /// Gets the name.
    /// </summary>
    public WorkspaceName Name { get; private set; }

    /// <summary>
    /// Creates a new <see cref="Workspace"/> entity.
    /// </summary>
    /// <remarks>
    /// The method generates a new <see cref="WorkspaceId"/> for the entity.
    /// All value objects should be validated in this method.
    /// </remarks>
    public static Workspace Create(WorkspaceName name)
    {
        var id = new WorkspaceId(Guid.NewGuid());

        return new Workspace(id, name);
    }
}
