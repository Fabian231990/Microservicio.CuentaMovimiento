using Microservicio.CuentaMovimiento.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.CuentaMovimiento.Infraestructura.Persistencia
{
    public class EjercicioTecnicoDBContext(DbContextOptions<EjercicioTecnicoDBContext> options) : DbContext(options)
    {
        /// <summary>
        /// Tabla de Cuentas en la base de datos.
        /// </summary>
        public DbSet<CuentaEntidad> Cuenta { get; set; }

        /// <summary>
        /// Tabla de Movimientos en la base de datos.
        /// </summary>
        public DbSet<MovimientoEntidad> Movimiento { get; set; }
    }
}
