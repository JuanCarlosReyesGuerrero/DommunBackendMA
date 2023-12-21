using DommunBackend.DomainLayer.Models;

namespace DommunBackend.RepositoryLayer.IRepository
{
    public interface IRepositorioComentarios
    {
        Task<List<Comentario>> ObtenerTodos(int peliculaId);
        Task<Comentario?> ObtenerPorId(int id);
        Task<int> Crear(Comentario comentario);
        Task Actualizar(Comentario comentario);
        Task<bool> Existe(int id);
        Task Borrar(int id);        
    }
}
