using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GastroWorld.Middleware
{
    public class VerificarSesionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<VerificarSesionMiddleware> _logger;

        public VerificarSesionMiddleware(
            RequestDelegate next,
            ILogger<VerificarSesionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var rutaActual = context.Request.Path.Value?.ToLower() ?? "";

            _logger.LogWarning($"RUTA COMPLETA: {rutaActual}");

            // Rutas completamente excluidas de verificación de sesión
            var rutasExcluidasExactas = new[]
            {
                "/",
                "/api/pgprincipal/cargarview",
                "/api/sobrenosotros/sobrenosotros",
                "/api/login/login",
                "/api/login/login/",
                "/api/registro/registro"
            };

            // Prefijos de rutas que no requieren verificación
            var prefijosExcluidos = new[]
            {
                "/api/login",
                "/api/registro"
            };

            // Verificar si la ruta es una exclusión exacta
            bool esRutaExcluidaExacta = rutasExcluidasExactas.Any(ruta =>
                rutaActual == ruta);

            // Verificar si la ruta comienza con algún prefijo excluido
            bool esPrefijoExcluido = prefijosExcluidos.Any(prefijo =>
                rutaActual.StartsWith(prefijo));

            // Si es una ruta excluida, continuar sin verificación
            if (esRutaExcluidaExacta || esPrefijoExcluido)
            {
                _logger.LogWarning($"Ruta EXCLUIDA: {rutaActual}");
                await _next(context);
                return;
            }

            // Verificación de sesión para rutas no excluidas
            var idUsuario = context.Session.GetInt32("id_usuario");

            _logger.LogWarning($"ID USUARIO EN SESIÓN: {idUsuario}");

            if (idUsuario == null)
            {
                _logger.LogWarning($"NO HAY SESIÓN ACTIVA para ruta: {rutaActual}");

                // Rechazar acceso si no hay sesión y no es una ruta de login
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("No autorizado. Inicie sesión.");
                return;
            }

            await _next(context);
        }
    }
}