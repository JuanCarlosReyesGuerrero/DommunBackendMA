namespace DommunBackend.DomainLayer.DTOs
{
    public class PaginacionDto
    {
        public int Pagina { get; set; }
        private int registrosPorPagina = 10;
        private readonly int cantidadMaximaRegistrosPorPagina = 50;

        public int RegistrosPorPagina
        {
            get
            {
                return registrosPorPagina;
            }
            set
            {
                registrosPorPagina = (value > cantidadMaximaRegistrosPorPagina) ? cantidadMaximaRegistrosPorPagina : value;
            }
        }
    }
}
