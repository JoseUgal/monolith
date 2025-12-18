using Application.Messaging;

namespace Modules.Tenants.Application.Tenants.Create;

/// <summary>
/// Represents the command for creating a new tenant.
/// </summary>
/// <param name="UserId">The user identifier.</param>
/// <param name="Name">The name.</param>
public sealed record CreateTenantCommand(
    Guid UserId,
    string Name
) : ICommand<Guid>;
