using DommunBackend.DomainLayer.DTOs;
using DommunBackend.Filtros;
using DommunBackend.Utilidades;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DommunBackend.EndPoints
{
    public static class UsuariosEndPoints
    {
        public static RouteGroupBuilder MapUsuarios(this RouteGroupBuilder group)
        {
            group.MapPost("/registrar", Registrar).AddEndpointFilter<FiltroValidaciones<CredencialesUsuarioDto>>();
            group.MapPost("/login", Login).AddEndpointFilter<FiltroValidaciones<CredencialesUsuarioDto>>();

            return group;
        }

        static async Task<Results<Ok<RespuestaAutenticacionDto>, BadRequest<IEnumerable<IdentityError>>>> Registrar(
            CredencialesUsuarioDto credencialesUsuarioDto, [FromServices] UserManager<IdentityUser> usermanager, IConfiguration configuration)
        {
            var usuario = new IdentityUser
            {
                UserName = credencialesUsuarioDto.Email,
                Email = credencialesUsuarioDto.Email
            };

            var resultado = await usermanager.CreateAsync(usuario, credencialesUsuarioDto.Password);

            if (resultado.Succeeded)
            {
                var credencialesRespuesta = ConstruirToken(credencialesUsuarioDto, configuration);

                return TypedResults.Ok(credencialesRespuesta);
            }
            else
            {
                return TypedResults.BadRequest(resultado.Errors);
            }
        }

        static async Task<Results<Ok<RespuestaAutenticacionDto>, BadRequest<string>>> Login(
            CredencialesUsuarioDto credencialesUsuarioDto, [FromServices] SignInManager<IdentityUser> signInManager,
            [FromServices] UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            var usuario = await userManager.FindByEmailAsync(credencialesUsuarioDto.Email);

            if (usuario is null)
            {
                return TypedResults.BadRequest("Login incorrecto");
            }

            var resultado = await signInManager.CheckPasswordSignInAsync(usuario, credencialesUsuarioDto.Password, lockoutOnFailure: false);


            if (resultado.Succeeded)
            {
                var respuestaAutenticacion = ConstruirToken(credencialesUsuarioDto, configuration);

                return TypedResults.Ok(respuestaAutenticacion);
            }
            else
            {
                return TypedResults.BadRequest("Login incorrecto");
            }
        }

        private static RespuestaAutenticacionDto ConstruirToken(CredencialesUsuarioDto credencialesUsuarioDto, IConfiguration configuration)
        {
            var claims = new List<Claim>
            {
                new Claim("email",credencialesUsuarioDto.Email),
                new Claim("lo que yo quiera","cualquier otro valor")
            };

            var llave = LlavesAutenticacion.ObtenerLlave(configuration);
            var creds = new SigningCredentials(llave.First(), SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddMinutes(2);

            var tokenDeSeguridad = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion,
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDeSeguridad);

            return new RespuestaAutenticacionDto
            {
                Token = token,
                Expiracion = expiracion
            };
        }
    }
}
