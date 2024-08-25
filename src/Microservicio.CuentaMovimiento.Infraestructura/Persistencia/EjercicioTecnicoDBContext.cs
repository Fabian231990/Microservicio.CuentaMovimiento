using Microsoft.EntityFrameworkCore;

namespace Microservicio.CuentaMovimiento.Infraestructura.Persistencia
{
    public class EjercicioTecnicoDBContext(DbContextOptions<EjercicioTecnicoDBContext> options) : DbContext(options)
    {
    }
}
