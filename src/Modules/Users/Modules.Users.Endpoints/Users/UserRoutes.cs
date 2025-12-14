namespace Modules.Users.Endpoints.Users;

internal static class UserRoutes
{
    internal const string Tag = "Users";

    internal const string BaseUri = "users";

    internal const string ResourceId = "userId";

    internal const string Register = $"{BaseUri}/register";
    
    internal const string GetById = $"{BaseUri}/{{{ResourceId}:guid}}";
    
    internal const string Update = $"{BaseUri}/{{{ResourceId}:guid}}";
}
