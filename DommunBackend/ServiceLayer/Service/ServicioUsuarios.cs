using DommunBackend.ServiceLayer.IService;
using Microsoft.AspNetCore.Identity;

namespace DommunBackend.ServiceLayer.Service
{
    public class ServicioUsuarios : IServicioUsuarios
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public ServicioUsuarios(IHttpContextAccessor contextAccessor, UserManager<IdentityUser> userManager)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<IdentityUser> Obtenerusuario()
        {
            var emailClaim = _contextAccessor.HttpContext!.User.Claims.Where(x => x.Type == "email").FirstOrDefault();

            if (emailClaim is null)
            {
                return null;
            }

            var email = emailClaim.Value;

            return await _userManager.FindByEmailAsync(email);
        }
    }
}
