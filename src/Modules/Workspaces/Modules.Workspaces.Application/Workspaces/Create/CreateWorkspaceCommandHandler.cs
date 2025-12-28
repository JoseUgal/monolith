using Application.Messaging;
using Domain.Errors;
using Domain.Results;
using Modules.Workspaces.Domain;
using Modules.Workspaces.Domain.Workspaces;

namespace Modules.Workspaces.Application.Workspaces.Create;

/// <summary>
/// Represents the <see cref="CreateWorkspaceCommand"/> handler.
/// </summary>
internal sealed class CreateWorkspaceCommandHandler(IWorkspaceRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<CreateWorkspaceCommand, Guid>
{
    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(CreateWorkspaceCommand command, CancellationToken cancellationToken)
    {
        if (!WorkspaceName.Create(command.Name).TryGetValue(out WorkspaceName name, out Error error))
        {
            return Result.Failure<Guid>(error);
        }

        Result<Workspace> workspace = Workspace.Create(command.TenantId, name, command.UserId);

        if (workspace.IsFailure)
        {
            return Result.Failure<Guid>(workspace.Error);
        }
        
        repository.Add(workspace.Value);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return workspace.Value.Id.Value;
    }
}
