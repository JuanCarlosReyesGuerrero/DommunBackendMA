using Microsoft.AspNetCore.Identity;

namespace DommunBackend.ServiceLayer.IService
{
    public interface IServicioUsuarios
    {
        Task<IdentityUser> Obtenerusuario();
    }
}