using DommunBackend.DomainLayer.DTOs;
using DommunBackend.RepositoryLayer.IRepository;
using FluentValidation;

namespace DommunBackend.Validaciones
{
    public class CrearGeneroDtoValidador : AbstractValidator<CrearGeneroDto>
    {
        public CrearGeneroDtoValidador(IRepositorioGeneros repositorioGeneros, IHttpContextAccessor httpContextAccessor)
        {
            var valorDeRutaId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
            var id = 0;

            if (valorDeRutaId is string valorString)
            {
                int.TryParse(valorString, out id);
            }

            RuleFor(x => x.Nombre).NotEmpty().WithMessage(UtilidadesValidacion.CampoRequeridoMensaje)
                .MaximumLength(50).WithMessage(UtilidadesValidacion.MaximumLengthMensaje)
                .Must(UtilidadesValidacion.PrimeletraEnMayuscula).WithMessage(UtilidadesValidacion.PrimeraLetraMayusculaMensaje)
                .MustAsync(async (nombre, _) =>
                {
                    var existe = await repositorioGeneros.Existe(id, nombre);

                    return !existe;
                }).WithMessage(g => $"Ya existe un género con el nombre {g.Nombre}");
        }
    }
}
