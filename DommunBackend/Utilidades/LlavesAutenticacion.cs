using Microsoft.IdentityModel.Tokens;

namespace DommunBackend.Utilidades
{
    public class LlavesAutenticacion
    {
        public const string IssuerPropio = "nuestra-app";
        public const string SeccionLlaves = "Authentication:Schemes:Bearer:SigningKeys";
        public const string SeccionLlaves_Emisor = "Issuer";
        public const string SeccionLlaves_Valor = "value";

        public static IEnumerable<SecurityKey> ObtenerLlave(IConfiguration configuration) => ObtenerLlave(configuration, IssuerPropio);

        public static IEnumerable<SecurityKey> ObtenerLlave(IConfiguration configuration, string issuer)
        {
            var signingKey = configuration.GetSection(SeccionLlaves)
                .GetChildren()
                .SingleOrDefault(llave => llave[SeccionLlaves_Emisor] == issuer);

            if (signingKey is not null && signingKey[SeccionLlaves_Valor] is string valorLlave)
                yield return new SymmetricSecurityKey(Convert.FromBase64String(valorLlave));
        }

        public static IEnumerable<SecurityKey> ObtenerTodasLasLlaves(IConfiguration configuration, string issuer)
        {
            var signingKeys = configuration.GetSection(SeccionLlaves)
                .GetChildren();

            foreach (var signingKey in signingKeys)
            {
                if (signingKey[SeccionLlaves_Valor] is string valorLlave)
                    yield return new SymmetricSecurityKey(Convert.FromBase64String(valorLlave));
            }
        }
    }
}
