using Domain.Primitives;
using Domain.Results;

namespace Modules.Workspaces.Domain.Workspaces;

public sealed class WorkspaceName : ValueObject
{
    /// <summary>
    /// Gets the maximum allowed length.
    /// </summary>
    public const int MaxLength = 80;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkspaceName"/> class.
    /// </summary>
    /// <param name="value">The validated workspace name.</param>
    /// <remarks>
    /// This constructor is intended for EF Core and mapping purposes only.
    /// It performs no validation. For domain-level creation and validation,
    /// use <see cref="Create(string)"/> instead.
    /// </remarks>
    public WorkspaceName(string value) => Value = value;
    
    /// <summary>
    /// Gets the value.
    /// </summary>
    public string Value { get; }
    
    protected override IEnumerable<object> GetAtomicValues() => [Value];

    /// <summary>
    /// Creates a validated workspace name.
    /// </summary>
    /// <param name="name">The primitive string value.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing either a valid <see cref="WorkspaceName"/>
    /// or an error describing why validation failed.
    /// </returns>
    public static Result<WorkspaceName> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<WorkspaceName>(
                WorkspaceErrors.Name.IsRequired
            );
        }
        
        name = name.Trim();

        if (name.Length > MaxLength)
        {
            return Result.Failure<WorkspaceName>(
                WorkspaceErrors.Name.TooLong(MaxLength)
            );
        }
        
        return new WorkspaceName(name);
    }
}
