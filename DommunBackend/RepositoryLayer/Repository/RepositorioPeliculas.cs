using AutoMapper;
using DommunBackend.DomainLayer.DTOs;
using DommunBackend.DomainLayer.Models;
using DommunBackend.RepositoryLayer.Data;
using DommunBackend.RepositoryLayer.IRepository;
using DommunBackend.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace DommunBackend.RepositoryLayer.Repository
{
    public class RepositorioPeliculas : IRepositorioPeliculas
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpContext _httpContext;
        private readonly IMapper _mapper;

        public RepositorioPeliculas(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _httpContext = httpContextAccessor.HttpContext!;
            _mapper = mapper;
        }

        public async Task<List<Pelicula>> ObtenerTodos(PaginacionDto paginacionDto)
        {
            var queryable = _context.Peliculas.AsQueryable();
            await _httpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            return await queryable.OrderBy(x => x.Titulo).Paginar(paginacionDto).ToListAsync();
        }

        public async Task<Pelicula?> ObtenerPorId(int id)
        {
            return await _context.Peliculas
                .Include(p => p.Comentarios)
                .Include(p => p.GeneroPeliculas)
                    .ThenInclude(gp => gp.Genero)
                .Include(p => p.ActoresPeliculas.OrderBy(a => a.Orden))
                    .ThenInclude(ap => ap.Actor)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> Crear(Pelicula genero)
        {
            _context.Add(genero);
            await _context.SaveChangesAsync();

            return genero.Id;
        }

        public async Task Actualizar(Pelicula genero)
        {
            _context.Update(genero);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Existe(int id)
        {
            return await _context.Peliculas.AnyAsync(x => x.Id == id);
        }

        public async Task Borrar(int id)
        {
            await _context.Peliculas.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Pelicula>> ObtenerPorNombre(string nombre)
        {
            return await _context.Peliculas
                .Where(a => a.Titulo.Contains(nombre))
                .OrderBy(a => a.Titulo)
                .ToListAsync();
        }

        public async Task AsignarGeneros(int id, List<int> generosIds)
        {
            var pelicula = await _context.Peliculas
                .Include(x => x.GeneroPeliculas)
                .FirstOrDefaultAsync();

            if (pelicula is null)
            {
                throw new Exception($"No existe una película con el id {id}");
            }

            var generosPeliculas = generosIds.Select(generoId => new GeneroPelicula() { GeneroId = generoId });

            pelicula.GeneroPeliculas = _mapper.Map(generosPeliculas, pelicula.GeneroPeliculas);

            await _context.SaveChangesAsync();
        }

        public async Task AsignarActores(int id, List<ActorPelicula> actores)
        {
            for (int i = 1; i <= actores.Count; i++)
            {
                actores[i - 1].Orden = i;
            }

            var pelicula = await _context.Peliculas
                .Include(p => p.ActoresPeliculas)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                throw new Exception($"No existe una película con el id {id}");
            }

            pelicula.ActoresPeliculas = _mapper.Map(actores, pelicula.ActoresPeliculas);

            await _context.SaveChangesAsync();
        }
    }
}
