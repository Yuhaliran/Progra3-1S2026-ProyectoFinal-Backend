namespace ProyectoFinal.Progra3.Backend
{
    using DbUp;
    using System.Data;
    using System.Reflection;
    using Microsoft.Data.SqlClient;
    using ProyectoFinal.Progra3.Backend.Repositorios.Interfaces;
    using ProyectoFinal.Progra3.Backend.Repositorios.Repositorios;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var conexionMigracion = builder.Configuration.GetConnectionString("ConexionMigracion");

            var upgrader = DeployChanges.To
                .SqlDatabase(conexionMigracion)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error en la migraicon de la db " + result.Error);
                Console.ResetColor();
                return;
            }

            var conexionPredeterminada = builder.Configuration.GetConnectionString("ConexionPredeterminada");

            builder.Services.AddScoped<IDbConnection>(sp =>new SqlConnection(conexionPredeterminada));

            // Add services to the container.
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

#if DEBUG
            builder.WebHost.UseUrls("https://localhost:6153", "http://localhost:6154");
#endif

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}