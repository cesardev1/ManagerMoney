using System.Security.Claims;

namespace ManagerMoney.Services
{
    public class UserServices : IUserServices
    {
        private readonly HttpContext httpContext;

        public UserServices(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }
        public int GetUserId()
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var idClaim = httpContext.User.Claims.Where(x=> x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                var id = int.Parse(idClaim.Value);
                return id;
            }
            else
            {
                throw new ApplicationException("El usuario no esta autenticado");
            }
            
        }
    }

    public interface IUserServices
    {
        int GetUserId();
    }
}
