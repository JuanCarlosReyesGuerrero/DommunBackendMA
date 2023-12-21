using DommunBackend.DomainLayer.DTOs;
using DommunBackend.RepositoryLayer.IRepository;
using FluentValidation;

namespace DommunBackend.Validaciones
{
    public class CrearActorDtoValidador : AbstractValidator<CrearActorDto>
    {
        public CrearActorDtoValidador(IRepositorioGeneros repositorioGeneros, IHttpContextAccessor httpContextAccessor)
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage(UtilidadesValidacion.CampoRequeridoMensaje)
                .MaximumLength(150).WithMessage(UtilidadesValidacion.MaximumLengthMensaje);

            var fechaMinima = new DateTime(1900, 1, 1);

            RuleFor(x => x.FechaNacimiento).GreaterThanOrEqualTo(fechaMinima)
                .WithMessage(UtilidadesValidacion.GreaterThanOrEqualToMensaje(fechaMinima));
        }
    }
}
