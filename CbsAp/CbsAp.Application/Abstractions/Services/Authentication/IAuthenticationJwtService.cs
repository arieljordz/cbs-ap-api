namespace CbsAp.Application.Abstractions.Services.Authentication
{
    public interface IAuthenticationJwtService
    {
        //TODO : check we can implement the user info class
        string GenerateUserJwtToken(string username, string role, decimal authorisationLimit, string[] permissions);
        
    }
}