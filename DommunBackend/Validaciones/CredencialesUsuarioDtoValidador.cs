using DommunBackend.DomainLayer.DTOs;
using FluentValidation;

namespace DommunBackend.Validaciones
{
    public class CredencialesUsuarioDtoValidador : AbstractValidator<CredencialesUsuarioDto>
    {
        public CredencialesUsuarioDtoValidador()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(UtilidadesValidacion.CampoRequeridoMensaje)
                .MaximumLength(256).WithMessage(UtilidadesValidacion.MaximumLengthMensaje)
                .EmailAddress().WithMessage(UtilidadesValidacion.EmailMensaje);

            RuleFor(x => x.Password).NotEmpty().WithMessage(UtilidadesValidacion.CampoRequeridoMensaje);
        }
    }
}
