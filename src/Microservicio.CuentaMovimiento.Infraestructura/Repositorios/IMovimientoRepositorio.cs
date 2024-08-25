using Microservicio.CuentaMovimiento.Dominio.Entidades;

namespace Microservicio.CuentaMovimiento.Infraestructura.Repositorios
{
    /// <summary>
    /// Interfaz Repositorio para la entidad Movimiento que interactúa con la base de datos.
    /// </summary>
    public interface IMovimientoRepositorio
    {
        /// <summary>
        /// Guarda un nuevo movimiento en la base de datos.
        /// </summary>
        /// <param name="movimientoEntidad">La entidad Movimiento a guardar.</param>
        /// <returns>El objeto MovimientoEntidad guardado.</returns>
        Task<MovimientoEntidad> GuardarMovimientoAsync(MovimientoEntidad movimientoEntidad);

        /// <summary>
        /// Elimina todos los movimientos asociados a una cuenta específica.
        /// </summary>
        /// <param name="idCuenta">El ID de la cuenta cuyos movimientos serán eliminados.</param>
        Task EliminarMovimientosPorCuentaAsync(int idCuenta);
    }
}
