using DommunBackend.DependencyInjection;
using DommunBackend.DomainLayer.Models;
using DommunBackend.EndPoints;
using DommunBackend.RepositoryLayer.Data;
using DommunBackend.RepositoryLayer.IRepository;
using DommunBackend.Utilidades;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var vOrigenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos")!;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Connection String  

builder.Services.ConexionDataBases(builder.Configuration);

#endregion

builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<SignInManager<IdentityUser>>();

#region Services Injected  

builder.Services.InyeccionServicios(builder.Configuration);

#endregion

builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(configuracion =>
    {
        configuracion.WithOrigins(vOrigenesPermitidos).AllowAnyHeader().AllowAnyMethod();
    });

    opciones.AddPolicy("libre", configuracion =>
    {
        configuracion.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddOutputCache();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Dommun API",
        Description = "Esta es una web api para la plataforma inmobiliaria Dommun",
        Contact = new OpenApiContact
        {
            Email = "quinde@gmail.com",
            Name = "Juan Reyes",
            Url = new Uri("http://quinde.co")
        },
        License = new OpenApiLicense
        {
            Name = "Privada",
            Url = new Uri("pagina licencia")
        }
    });
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddProblemDetails();

builder.Services.AddAuthentication()
    .AddJwtBearer(opciones =>
    {
        opciones.MapInboundClaims = false;

        opciones.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = LlavesAutenticacion.ObtenerLlave(builder.Configuration).First(),
            //IssuerSigningKeys = LlavesAutenticacion.ObtenerTodasLasLlaves(builder.Configuration)
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization(opciones =>
    {
        opciones.AddPolicy("esAdmin", politica => politica.RequireClaim("esAdmin"));
    });








var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.Run(async context =>
{
    var exeptionHandleFeature = context.Features.Get<IExceptionHandlerFeature>();
    var excepcion = exeptionHandleFeature?.Error!;

    var mensajeError = new MensajeError()
    {
        Fecha = DateTime.Now,
        MensajeDeError = excepcion.Message,
        StackTrace = excepcion.StackTrace
    };

    var repositorio = context.RequestServices.GetRequiredService<IRepositorioMensajeErrores>();
    await repositorio.CrearError(mensajeError);

    await TypedResults
    .BadRequest(new { tipo = "error", mensaje = "ha ocurrido un mensaje inesperado", estatus = 500 })
    .ExecuteAsync(context);
}));
app.UseStatusCodePages();

app.UseStaticFiles();

app.UseCors();

app.UseOutputCache();

app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
//app.MapGet("/", [EnableCors(policyName:"libre")]() => "Hello World!");

app.MapGroup("/generos").MapGeneros();
app.MapGroup("/actores").MapActores();
app.MapGroup("/peliculas").MapPeliculas();
app.MapGroup("/pelicula/{peliculaId:int}/comentarios").MapComentarios();
app.MapGroup("/usuarios").MapUsuarios();

app.Run();