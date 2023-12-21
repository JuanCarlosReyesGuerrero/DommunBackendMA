using DommunBackend.DomainLayer.DTOs;
using DommunBackend.DomainLayer.Models;
using DommunBackend.RepositoryLayer.Data;
using DommunBackend.RepositoryLayer.IRepository;
using DommunBackend.Utilidades;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace DommunBackend.RepositoryLayer.Repository
{
    public class RepositorioActores : IRepositorioActores
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpContext _httpContext;

        public RepositorioActores(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<List<Actor>> ObtenerTodos(PaginacionDto paginacionDto)
        {
            var queryable = _context.Actores.AsQueryable();
            await _httpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            return await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDto).ToListAsync();
        }

        public async Task<Actor?> ObtenerPorId(int id)
        {
            return await _context.Actores.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> Crear(Actor genero)
        {
            _context.Add(genero);
            await _context.SaveChangesAsync();

            return genero.Id;
        }

        public async Task Actualizar(Actor genero)
        {
            _context.Update(genero);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Actores.AnyAsync(x => x.Id == id);
        }

        public async Task<List<int>> Existen(List<int> ids)
        {
            return await _context.Actores.Where(a=>ids.Contains(a.Id)).Select(a=>a.Id).ToListAsync();
        }

        public async Task Borrar(int id)
        {
            await _context.Actores.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Actor>> ObtenerPorNombre(string nombre)
        {
            return await _context.Actores
                .Where(a => a.Nombre.Contains(nombre))
                .OrderBy(a => a.Nombre)
                .ToListAsync();
        }
    }
}
