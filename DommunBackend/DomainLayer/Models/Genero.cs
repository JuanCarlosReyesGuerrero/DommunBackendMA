namespace DommunBackend.DomainLayer.Models
{
    public class Genero : BaseEntity
    {
        public string? Nombre { get; set; } = null!;

        public List<GeneroPelicula> GeneroPeliculas { get; set; } = new List<GeneroPelicula>();
    }
}
