namespace DommunBackend.DomainLayer.Models
{
    public class MensajeError
    {
        public Guid Id { get; set; }
        public string MensajeDeError { get; set; } = null!;
        public string? StackTrace { get; set; }
        public DateTime Fecha { get; set; }
    }
}
