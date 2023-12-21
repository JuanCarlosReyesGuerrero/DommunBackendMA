namespace DommunBackend.DomainLayer.Models
{
    public class Actor : BaseEntity
    {
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Foto { get; set; }

        public List<ActorPelicula> ActoresPeliculas { get; set; } = new List<ActorPelicula>();
    }
}
