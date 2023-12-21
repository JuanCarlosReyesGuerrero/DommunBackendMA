using DommunBackend.DomainLayer.DTOs;
using FluentValidation;

namespace DommunBackend.Validaciones
{
    public class EditarClaimDtoValidacion : AbstractValidator<EditarClaimDto>
    {
        public EditarClaimDtoValidacion()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(UtilidadesValidacion.CampoRequeridoMensaje)
               .MaximumLength(256).WithMessage(UtilidadesValidacion.MaximumLengthMensaje)
               .EmailAddress().WithMessage(UtilidadesValidacion.EmailMensaje);
        }
    }
}
