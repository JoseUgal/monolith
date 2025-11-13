using App;
using Infrastructure.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.InstallServicesFromAssemblies(
    builder.Configuration,
    AssemblyReference.Assembly,
    Infrastructure.AssemblyReference.Assembly
);

WebApplication app = builder.Build();

app.UseHttpsRedirection();

app.Run();
