using System.Security.Claims;

namespace MusiCom.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static Guid Id(this ClaimsPrincipal user)
        {
            string id = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
            {
                return Guid.Empty;
            }

            return Guid.Parse(id);
        }
    }
}
