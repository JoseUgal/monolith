using Application.Messaging;
using Domain.Results;
using Modules.Workspaces.Domain;
using Modules.Workspaces.Domain.WorkspaceMemberships;
using Modules.Workspaces.Domain.Workspaces;

namespace Modules.Workspaces.Application.Workspaces.InviteMember;

/// <summary>
/// Represents the <see cref="InviteWorkspaceMemberCommand"/> handler.
/// </summary>
internal sealed class InviteWorkspaceMemberCommandHandler(IWorkspaceRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<InviteWorkspaceMemberCommand, Guid>
{
    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(InviteWorkspaceMemberCommand command, CancellationToken cancellationToken)
    {
        var workspaceId = new WorkspaceId(command.WorkspaceId);
        
        Workspace? workspace = await repository.GetWithMembershipsAsync(workspaceId, cancellationToken);

        if (workspace == null)
        {
            return Result.Failure<Guid>(
                WorkspaceErrors.NotFound(workspaceId)
            );
        }

        if (!Enum.TryParse(command.Role, ignoreCase: true, out WorkspaceMembershipRole role))
        {
            return Result.Failure<Guid>(
                WorkspaceMembershipErrors.Role.IsInvalid
            );
        }

        Result<WorkspaceMembership> invite = workspace.InviteMember(command.UserId, role);

        if (invite.IsFailure)
        {
            return Result.Failure<Guid>(invite.Error);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return invite.Value.Id.Value;
    }
}
