using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using TheCollabSys.Backend.API.Filters;

namespace TheCollabSys.Backend.API.Configuration
{
    public class PresentationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            // Configuración de CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin() // Permite cualquier origen
                           .AllowAnyMethod() // Permite cualquier método HTTP
                           .AllowAnyHeader() // Permite cualquier encabezado
                           .WithExposedHeaders("Content-Disposition"); // Expone encabezados personalizados si es necesario
                });
            });

            // Configuración de controladores
            services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                options.Filters.Add(new ProducesAttribute("application/json", "application/xml"));
                options.Filters.Add(typeof(GlobalExceptionFilter));
                options.Filters.Add(typeof(ModelStateFilter));
            })
            .AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "The Collabsys API", Version = "v1" });

                // Configura la autenticación JWT en Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                // Agrega un esquema de seguridad para Swagger UI
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104857600;
            });
        }
    }
}
