namespace DommunBackend.DomainLayer.DTOs
{
    public class GeneroDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
