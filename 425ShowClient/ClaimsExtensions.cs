using System.Collections.Generic;
using System.Linq;

namespace Four25ShowClient
{
    public static class ClaimsExtensions
    {
        public static string GetClaimValue(this IEnumerable<System.Security.Claims.Claim> claims, string claimName)
        {
            if (claims == null || !claims.Any())
            {
                return string.Empty;
            }

            var foundClaim = claims.FirstOrDefault(c => c.Type == claimName);
            return foundClaim?.Value;
        }
    }
}
