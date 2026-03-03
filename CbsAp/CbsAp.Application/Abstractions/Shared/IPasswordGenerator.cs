using CbsAp.Application.DTOs.Shared;

namespace CbsAp.Application.Abstractions.Shared
{
    public interface IPasswordGenerator
    {
        string Generate(PasswordOptions options);
    }
}
