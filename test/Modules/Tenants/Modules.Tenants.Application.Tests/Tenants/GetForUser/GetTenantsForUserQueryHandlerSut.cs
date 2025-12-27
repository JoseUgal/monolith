using Application.Data;
using Modules.Tenants.Application.Tenants.GetForUser;
using Moq;

namespace Modules.Tenants.Application.Tests.Tenants.GetForUser;

internal sealed class GetTenantsForUserQueryHandlerSut
{
    public Mock<ISqlQueryExecutor> Sql { get; } = new(MockBehavior.Strict);

    public GetTenantsForUserQueryHandler Handler { get; }

    public GetTenantsForUserQueryHandlerSut()
    {
        Handler = new GetTenantsForUserQueryHandler(Sql.Object);
    }
}
