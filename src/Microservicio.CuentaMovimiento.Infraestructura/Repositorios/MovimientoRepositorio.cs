using Microservicio.CuentaMovimiento.Dominio.Dto;
using Microservicio.CuentaMovimiento.Dominio.Entidades;
using Microservicio.CuentaMovimiento.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.CuentaMovimiento.Infraestructura.Repositorios
{
    /// <summary>
    /// Repositorio para la gestion de movimientos en la base de datos.
    /// </summary>
    /// <remarks>
    /// Constructor que inicializa el repositorio con el contexto de la base de datos.
    /// </remarks>
    /// <param name="dbContext">Contexto de la base de datos.</param>
    public class MovimientoRepositorio(EjercicioTecnicoDBContext dbContext) : IMovimientoRepositorio
    {
        /// <summary>
        /// Contexto de base de datos.
        /// </summary>
        private readonly EjercicioTecnicoDBContext _dbContext = dbContext;

        /// <summary>
        /// Guarda un nuevo movimiento en la base de datos.
        /// </summary>
        /// <param name="movimientoDto">El DTO Movimiento a guardar.</param>
        /// <returns>El objeto MovimientoDto guardado.</returns>
        public async Task<MovimientoDto> GuardarMovimientoAsync(MovimientoDto movimientoDto)
        {
            var movimientoEntidad = new MovimientoEntidad
            {
                Fecha = movimientoDto.Fecha,
                TipoMovimiento = movimientoDto.TipoMovimiento,
                Valor = movimientoDto.Valor,
                Saldo = movimientoDto.Saldo,
                IdCuenta = movimientoDto.IdCuenta
            };

            await _dbContext.Movimientos.AddAsync(movimientoEntidad);
            await _dbContext.SaveChangesAsync();

            movimientoDto.IdMovimiento = movimientoEntidad.IdMovimiento;
            return movimientoDto;
        }

        /// <summary>
        /// Elimina todos los movimientos asociados a una cuenta especifica, identificada por su numero de cuenta.
        /// </summary>
        /// <param name="numeroCuenta">El numero de cuenta cuyos movimientos seran eliminados.</param>
        public async Task EliminarMovimientosPorCuentaAsync(string numeroCuenta)
        {
            var cuenta = await _dbContext.Cuenta.FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta) ?? throw new InvalidOperationException("La cuenta con el numero especificado no fue encontrada.");
            var movimientos = _dbContext.Movimientos.Where(m => m.IdCuenta == cuenta.IdCuenta);

            _dbContext.Movimientos.RemoveRange(movimientos);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Obtiene los movimientos de una cuenta en un rango de fechas.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta.</param>
        /// <param name="fechaInicio">Fecha de inicio del rango.</param>
        /// <param name="fechaFin">Fecha de fin del rango.</param>
        /// <returns>Listado de movimientos dentro del rango de fechas.</returns>
        public async Task<IEnumerable<MovimientoDto>> ObtenerMovimientosPorCuentaYRangoFechasAsync(string numeroCuenta, DateTime fechaInicio, DateTime fechaFin)
        {
            var movimientos = await _dbContext.Movimientos
                .Include(m => m.Cuenta)
                .Where(m => m.Cuenta.NumeroCuenta == numeroCuenta && m.Fecha >= fechaInicio && m.Fecha <= fechaFin)
                .ToListAsync();

            return movimientos.Select(m => new MovimientoDto
            {
                IdMovimiento = m.IdMovimiento,
                Fecha = m.Fecha,
                TipoMovimiento = m.TipoMovimiento,
                Valor = m.Valor,
                Saldo = m.Saldo,
                NumeroCuenta = m.Cuenta.NumeroCuenta,
                TipoCuenta = m.Cuenta.TipoCuenta
            });
        }
    }
}
