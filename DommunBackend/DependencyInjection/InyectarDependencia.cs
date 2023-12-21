using DommunBackend.RepositoryLayer.Data;
using DommunBackend.RepositoryLayer.IRepository;
using DommunBackend.RepositoryLayer.Repository;
using DommunBackend.ServiceLayer.IService;
using DommunBackend.ServiceLayer.Service;
using Microsoft.EntityFrameworkCore;

namespace DommunBackend.DependencyInjection
{
    public static class InyectarDependencia
    {
        public static void ConexionDataBases(this IServiceCollection services, IConfiguration Configuration)
        {
            string dbConnectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(dbConnectionString));
        }

        public static void InyeccionServicios(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped(typeof(IRepositorioGeneros), typeof(RepositorioGeneros));
            services.AddScoped(typeof(IRepositorioActores), typeof(RepositorioActores));
            services.AddScoped(typeof(IRepositorioPeliculas), typeof(RepositorioPeliculas));
            services.AddScoped(typeof(IRepositorioComentarios), typeof(RepositorioComentarios));
            services.AddScoped(typeof(IRepositorioMensajeErrores), typeof(RepositorioMensajeErrores));


            services.AddScoped(typeof(IAlmacenadorArchivos), typeof(AlmacenadorArchivosAzure));
            services.AddScoped(typeof(IAlmacenadorArchivos), typeof(AlmacenadorArchivosLocal));
            services.AddTransient(typeof(IServicioUsuarios), typeof(ServicioUsuarios));

            services.AddHttpContextAccessor();

            //services.AddScoped(typeof(IBackOfficeService), typeof(BackOfficeService));            
        }
    }
}
