using CbsAp.Application.Shared.Encryption;

namespace CbsAp.UnitTesting.SharedTest
{
    public class HashingAndSaltingPasswordsTests
    {
        [Fact]
        public void WhenHasingPassword_ThenReturnsHashAndSalt()
        {
            HashingandSaltingPassword hashingandSaltingPassword =
                new HashingandSaltingPassword();
            var hash = hashingandSaltingPassword
                    .HashPasword("neworUpdate_password", out var salt);

            Assert.NotNull(hash);
            Assert.NotNull(salt);
        }

        [Fact]
        public void WhenVerifyingPassword_ThenPositiveVerificationSucceeds()
        {
            HashingandSaltingPassword hashingandSaltingPassword =
                new HashingandSaltingPassword();
            var hash = hashingandSaltingPassword.HashPasword("accessVeried_password", out var salt);

            var verificationResult =
                hashingandSaltingPassword.VerifyPassword("accessVeried_password", hash, salt);

            Assert.True(verificationResult);
        }

        [Fact]
        public void WhenVerifyingPassword_ThenNegativeVerificationSucceeds()
        {
            HashingandSaltingPassword hashingandSaltingPassword =
               new HashingandSaltingPassword();
            var hash = hashingandSaltingPassword.HashPasword("accessVeried_password", out var salt);

            

            var verificationResult = hashingandSaltingPassword.VerifyPassword("wrong_password", hash, salt);

            Assert.False(verificationResult);
        }
    }
}