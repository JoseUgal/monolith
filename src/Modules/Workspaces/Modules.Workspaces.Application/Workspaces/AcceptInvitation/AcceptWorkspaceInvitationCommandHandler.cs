using Application.Messaging;
using Domain.Results;
using Modules.Workspaces.Domain;
using Modules.Workspaces.Domain.WorkspaceMemberships;
using Modules.Workspaces.Domain.Workspaces;

namespace Modules.Workspaces.Application.Workspaces.AcceptInvitation;

/// <summary>
/// Represents the <see cref="AcceptWorkspaceInvitationCommand"/> handler.
/// </summary>
internal sealed class AcceptWorkspaceInvitationCommandHandler(IWorkspaceRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<AcceptWorkspaceInvitationCommand>
{
    /// <inheritdoc />
    public async Task<Result> Handle(AcceptWorkspaceInvitationCommand command, CancellationToken cancellationToken)
    {
        var workspaceId = new WorkspaceId(command.WorkspaceId);
        
        Workspace? workspace = await repository.GetWithMembershipsAsync(workspaceId, cancellationToken);

        if (workspace == null)
        {
            return Result.Failure(
                WorkspaceErrors.NotFound(workspaceId)
            );
        }
        
        var membershipId = new WorkspaceMembershipId(command.MembershipId);

        Result result = workspace.AcceptInvitation(membershipId, command.UserId);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
