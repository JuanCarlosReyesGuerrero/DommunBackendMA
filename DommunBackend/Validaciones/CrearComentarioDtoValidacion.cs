using DommunBackend.DomainLayer.DTOs;
using FluentValidation;

namespace DommunBackend.Validaciones
{
    public class CrearComentarioDtoValidacion : AbstractValidator<CrearComentarioDto>
    {
        public CrearComentarioDtoValidacion()
        {
            RuleFor(x => x.Cuerpo).NotEmpty().WithMessage(UtilidadesValidacion.CampoRequeridoMensaje);
        }
    }
}
