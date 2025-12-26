using Application.Messaging;
using Domain.Errors;
using Domain.Results;
using Modules.Tenants.Domain;
using Modules.Tenants.Domain.Tenants;

namespace Modules.Tenants.Application.Tenants.Create;

/// <summary>
/// Represents the <see cref="CreateTenantCommand"/> handler.
/// </summary>
internal sealed class CreateTenantCommandHandler(ITenantRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<CreateTenantCommand, Guid>
{
    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(CreateTenantCommand command, CancellationToken cancellationToken)
    {
        if (!TenantName.Create(command.Name).TryGetValue(out TenantName name, out Error error))
        {
            return Result.Failure<Guid>(error);
        }
        
        if (!TenantSlug.Create(command.Slug).TryGetValue(out TenantSlug slug, out error))
        {
            return Result.Failure<Guid>(error);
        }

        if (!await repository.IsSlugUniqueAsync(slug, cancellationToken))
        {
            return Result.Failure<Guid>(TenantErrors.Slug.IsNotUnique);
        }

        Result<Tenant> tenant = Tenant.Create(name, slug, command.UserId);

        if (tenant.IsFailure)
        {
            return Result.Failure<Guid>(tenant.Error);
        }
        
        repository.Add(tenant.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return tenant.Value.Id.Value;
    }
}
