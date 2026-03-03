using CbsAp.Domain.Exceptions;

namespace CbsAp.Application.Exceptions
{
    public class AuthenticationException : AppException
    {
        public AuthenticationException(string title, string message) :
           base("Authentication Error", "One or more validation errors occurred")
        {
        }
    }
}