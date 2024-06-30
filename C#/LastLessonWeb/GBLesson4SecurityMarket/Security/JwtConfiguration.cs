using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GBLesson4SecurityMarket.Security
{
    public class JwtConfiguration
    {
        public required string Key { get; init; }
        public required string Issuer { get; init; }
        public required string Audience { get; init; }

        internal SymmetricSecurityKey GetSigningKey()
        {
            return new(Encoding.UTF8.GetBytes(Key));
        }
    }
}
