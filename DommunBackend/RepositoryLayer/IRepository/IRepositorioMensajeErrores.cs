using DommunBackend.DomainLayer.Models;

namespace DommunBackend.RepositoryLayer.IRepository
{
    public interface IRepositorioMensajeErrores
    {
        Task CrearError(MensajeError mensajeError);
    }
}