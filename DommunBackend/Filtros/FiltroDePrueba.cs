
using AutoMapper;
using DommunBackend.RepositoryLayer.IRepository;

namespace DommunBackend.Filtros
{
    public class FiltroDePrueba : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            //Este código se ejecuta antes del endpoint
            var paramRepositorioGeneros = context.Arguments.OfType<IRepositorioGeneros>().FirstOrDefault();
            var paramEntero = context.Arguments.OfType<int>().FirstOrDefault();
            var paramMapper = context.Arguments.OfType<IMapper>().FirstOrDefault();

            var resultado = await next(context);

            //este código se ejecuta despues del endpoint
            return resultado;
        }
    }
}
