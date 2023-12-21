namespace DommunBackend.DomainLayer.Models
{
    public class ActorPelicula
    {
        public int ActorId { get; set; }
        public int PeliculaId { get; set; }
        public int Orden { get; set; }
        public string Personaje { get; set; } = null!;

        public Actor Actor { get; set; } = null!;
        public Pelicula Pelicula { get; set; } = null!;
    }
}
