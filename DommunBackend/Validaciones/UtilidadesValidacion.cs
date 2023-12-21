namespace DommunBackend.Validaciones
{
    public static class UtilidadesValidacion
    {
        public static string CampoRequeridoMensaje = "El campo {PropertyName} es requerido";
        public static string MaximumLengthMensaje = "El campo {PropertyName} debe tener menos de {MaxLength} carateres";
        public static string PrimeraLetraMayusculaMensaje = "El campo {PropertyName} debe comenzar con mayúsculas";
        public static string EmailMensaje = "El campo {PropertyName} debe ser un email válido";

        public static string GreaterThanOrEqualToMensaje(DateTime fechaMinima)
        {
            return "El campo {PropertyName} debe ser posterior a " + fechaMinima.ToString("yyyy-MM-dd");
        }

        public static bool PrimeletraEnMayuscula(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return true;

            var primeraLetra = valor[0].ToString();

            return primeraLetra == primeraLetra.ToUpper();
        }
    }
}
