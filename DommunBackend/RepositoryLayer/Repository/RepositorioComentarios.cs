using DommunBackend.DomainLayer.Models;
using DommunBackend.RepositoryLayer.Data;
using DommunBackend.RepositoryLayer.IRepository;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace DommunBackend.RepositoryLayer.Repository
{
    public class RepositorioComentarios : IRepositorioComentarios
    {
        private readonly ApplicationDbContext _context;

        public RepositorioComentarios(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Comentario>> ObtenerTodos(int peliculaId)
        {
            return await _context.Comentarios.Where(c => c.PeliculaId == peliculaId).ToListAsync();
        }

        public async Task<Comentario?> ObtenerPorId(int id)
        {
            return await _context.Comentarios.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> Crear(Comentario comentario)
        {
            _context.Add(comentario);
            await _context.SaveChangesAsync();

            return comentario.Id;
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Comentarios.AnyAsync(c => c.Id == id);
        }

        public async Task Actualizar(Comentario comentario)
        {
            _context.Update(comentario);
            await _context.SaveChangesAsync();
        }

        public async Task Borrar(int id)
        {
            await _context.Comentarios.Where(c => c.Id == id).ExecuteDeleteAsync();
        }
    }
}
