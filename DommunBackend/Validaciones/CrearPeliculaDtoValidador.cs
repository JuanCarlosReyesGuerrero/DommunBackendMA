using DommunBackend.DomainLayer.DTOs;
using FluentValidation;

namespace DommunBackend.Validaciones
{
    public class CrearPeliculaDtoValidador : AbstractValidator<CrearPeliculaDto>
    {
        public CrearPeliculaDtoValidador()
        {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage(UtilidadesValidacion.CampoRequeridoMensaje)
                .MaximumLength(150).WithMessage(UtilidadesValidacion.MaximumLengthMensaje);
        }
    }
}
