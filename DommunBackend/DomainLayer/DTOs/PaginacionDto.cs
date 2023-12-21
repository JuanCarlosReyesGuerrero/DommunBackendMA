using DommunBackend.Utilidades;

namespace DommunBackend.DomainLayer.DTOs
{
    public class PaginacionDto
    {
        private const int paginaValorInicial = 1;
        private const int registrosPaginaValorInicial = 10;
        public int Pagina { get; set; } = paginaValorInicial;
        private int registrosPorPagina = registrosPaginaValorInicial;
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

        public static ValueTask<PaginacionDto> BindAsync(HttpContext context)
        {
            var pagina = context.ExtraerValorDefecto(nameof(Pagina), paginaValorInicial);
            var registrosPorPagina = context.ExtraerValorDefecto(nameof(RegistrosPorPagina), registrosPaginaValorInicial);

            var resultado = new PaginacionDto
            {
                Pagina = pagina,
                RegistrosPorPagina = registrosPorPagina
            };

            return ValueTask.FromResult(resultado);
        }
    }
}
