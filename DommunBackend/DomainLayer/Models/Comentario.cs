using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DommunBackend.DomainLayer.Models
{
    public class Comentario : BaseEntity
    {
        public string Cuerpo { get; set; }
        public int PeliculaId { get; set; }
    }
}
