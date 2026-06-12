using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Hope_tracKeR_back.Config;

public class AuthOptions
{
    public const string ISSUER = "TSMBack";
    public const string AUDIENCE = "TSMClient";
    const string KEY = "prostokeyfor1234231dsfsdq3shiphrovki014345";
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(KEY));
}