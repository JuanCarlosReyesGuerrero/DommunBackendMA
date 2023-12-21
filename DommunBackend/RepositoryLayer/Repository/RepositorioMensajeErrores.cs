using DommunBackend.DomainLayer.Models;
using DommunBackend.RepositoryLayer.Data;
using DommunBackend.RepositoryLayer.IRepository;

namespace DommunBackend.RepositoryLayer.Repository
{
    public class RepositorioMensajeErrores : IRepositorioMensajeErrores
    {
        private readonly ApplicationDbContext _context;
        public RepositorioMensajeErrores(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CrearError(MensajeError mensajeError)
        {
            _context.Add(mensajeError);
            await _context.SaveChangesAsync();
        }
    }
}
