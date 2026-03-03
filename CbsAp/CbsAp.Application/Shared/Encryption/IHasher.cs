using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Shared.Encryption
{
    public interface IHasher
    {
        string HashPasword(string password, out byte[] salt);
        bool VerifyPassword(string password, string storedHash, byte[] salt);
    }
}
