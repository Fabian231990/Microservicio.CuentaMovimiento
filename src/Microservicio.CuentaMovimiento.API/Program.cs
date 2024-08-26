using Microservicio.CuentaMovimiento.Aplicacion.Servicios;
using Microservicio.CuentaMovimiento.Infraestructura.Persistencia;
using Microservicio.CuentaMovimiento.Infraestructura.Repositorios;
using Microservicio.CuentaMovimiento.Infraestructura.Servicios;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder constructor = WebApplication.CreateBuilder(args);

constructor.Services.AddControllers();
constructor.Services.AddEndpointsApiExplorer();
constructor.Services.AddSwaggerGen();
constructor.Services.AddHttpClient();

constructor.Services.AddScoped<ICuentaRepositorio, CuentaRepositorio>();
constructor.Services.AddScoped<ICuentaServicio, CuentaServicio>();
constructor.Services.AddScoped<IClienteServicio, ClienteServicio>();
constructor.Services.AddScoped<IMovimientoRepositorio, MovimientoRepositorio>();
constructor.Services.AddScoped<IMovimientoServicio, MovimientoServicio>();

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
