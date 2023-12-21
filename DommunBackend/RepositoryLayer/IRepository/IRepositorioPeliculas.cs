using DommunBackend.DomainLayer.DTOs;
using DommunBackend.DomainLayer.Models;

namespace DommunBackend.RepositoryLayer.IRepository
{
    public interface IRepositorioPeliculas
    {
        Task Actualizar(Pelicula pelicula);
        Task AsignarActores(int id, List<ActorPelicula> actores);
        Task AsignarGeneros(int id, List<int> generosIds);
        Task Borrar(int id);
        Task<int> Crear(Pelicula pelicula);
        Task<bool> Existe(int id);
        Task<Pelicula?> ObtenerPorId(int id);
        Task<List<Pelicula>> ObtenerPorNombre(string nombre);
        Task<List<Pelicula>> ObtenerTodos(PaginacionDto paginacionDto);
    }
}
