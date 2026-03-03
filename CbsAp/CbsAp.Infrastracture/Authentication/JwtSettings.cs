namespace CbsAp.Infrastracture.Authentication
{
#nullable disable

    public class JWTSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}