using Microservicio.CuentaMovimiento.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder constructor = WebApplication.CreateBuilder(args);

constructor.Services.AddControllers();
constructor.Services.AddEndpointsApiExplorer();
constructor.Services.AddSwaggerGen();

constructor.Services.AddDbContext<EjercicioTecnicoDBContext>(options =>
    options.UseSqlServer(constructor.Configuration.GetConnectionString("Conexion")));

WebApplication aplicacion = constructor.Build();

if (aplicacion.Environment.IsDevelopment() || aplicacion.Environment.IsProduction())
{
    aplicacion.UseSwagger();
    aplicacion.UseSwaggerUI();
}

aplicacion.UseHttpsRedirection();
aplicacion.UseAuthorization();
aplicacion.MapControllers();
aplicacion.Run();
