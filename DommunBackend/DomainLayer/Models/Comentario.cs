using Microsoft.AspNetCore.Identity;

namespace DommunBackend.DomainLayer.Models
{
    public class Comentario : BaseEntity
    {
        public string Cuerpo { get; set; } = null!;
        public int PeliculaId { get; set; }
        public string UsuarioId { get; set; } = null!;
        public IdentityUser Usuario { get; set; } = null!;
    }
}
