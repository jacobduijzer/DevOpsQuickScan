namespace DevOpsQuickScan.Api.Application;

[ExtendObjectType("queries")]
public class SessionQueries
{
    // [GraphQLDescription("Login with an email and a password to retrieve a bearer token.")]
    // public async Task Login(string email, string password) => 
    //     await Task.CompletedTask;

    public string Get(string sessionCode) => sessionCode;
}