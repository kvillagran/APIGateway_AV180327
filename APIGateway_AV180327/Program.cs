using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

namespace APIGateway_AV180327
{
    public class Program
    {
        public static async Task Main(string[] args) // Hacer Main async para poder usar await
        {
            var builder = WebApplication.CreateBuilder(args);

            // Cargar la configuraci�n de Ocelot desde el archivo ocelot.json
            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
          options.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ValidIssuer = "https://myauthserver.com",
              ValidAudience = "https://myauthserver.com",
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretKey@345"))
          };
      });


            // Agregar los servicios de Ocelot
            builder.Services.AddOcelot();

            var app = builder.Build();

            // Habilitar autenticaci�n y autorizaci�n
            app.UseAuthentication();
            app.UseAuthorization();

            // Usar el middleware de Ocelot (con await correctamente)
            await app.UseOcelot();

            // Endpoint de prueba
            app.MapGet("/", () => "API Gateway en ejecuci�n con Ocelot");

            // Iniciar la aplicaci�n
            app.Run();
        }
    }
}
