using Microservicio.CuentaMovimiento.Dominio.Entidades;
using Microservicio.CuentaMovimiento.Infraestructura.Persistencia;

namespace Microservicio.CuentaMovimiento.Infraestructura.Repositorios
{
    public class MovimientoRepositorio(EjercicioTecnicoDBContext ejercicioTecnicoDBContext) : IMovimientoRepositorio
    {
        private readonly EjercicioTecnicoDBContext ejercicioTecnicoDBContext = ejercicioTecnicoDBContext;

        /// <summary>
        /// Guarda un nuevo movimiento en la base de datos.
        /// </summary>
        /// <param name="movimientoEntidad">La entidad Movimiento a guardar.</param>
        /// <returns>El objeto MovimientoEntidad guardado.</returns>
        public async Task<MovimientoEntidad> GuardarMovimientoAsync(MovimientoEntidad movimientoEntidad)
        {
            await ejercicioTecnicoDBContext.Movimiento.AddAsync(movimientoEntidad);
            await ejercicioTecnicoDBContext.SaveChangesAsync();
            return movimientoEntidad;
        }

        /// <summary>
        /// Elimina todos los movimientos asociados a una cuenta específica.
        /// </summary>
        /// <param name="idCuenta">El ID de la cuenta cuyos movimientos serán eliminados.</param>
        public async Task EliminarMovimientosPorCuentaAsync(int idCuenta)
        {
            var movimientos = ejercicioTecnicoDBContext.Movimiento.Where(m => m.IdCuenta == idCuenta);
            ejercicioTecnicoDBContext.Movimiento.RemoveRange(movimientos);
            await ejercicioTecnicoDBContext.SaveChangesAsync();
        }
    }
}
