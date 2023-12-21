using Microsoft.IdentityModel.Tokens;

namespace DommunBackend.Utilidades
{
    public static class HttpContextExtensionsUtilidades
    {
        public static T ExtraerValorDefecto<T>(this HttpContext context, string nombreDelCampo, T valorPorDefecto) where T : IParsable<T>
        {
            var valor = context.Request.Query[nombreDelCampo];

            if (valor.IsNullOrEmpty())
            {

            }

            return T.Parse(valor!, null);
        }
    }
}
