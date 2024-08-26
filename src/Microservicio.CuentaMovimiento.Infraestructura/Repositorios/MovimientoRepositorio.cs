using Microservicio.CuentaMovimiento.Dominio.Dto;
using Microservicio.CuentaMovimiento.Dominio.Entidades;
using Microservicio.CuentaMovimiento.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.CuentaMovimiento.Infraestructura.Repositorios
{
    /// <summary>
    /// Repositorio para la gestion de movimientos en la base de datos.
    /// </summary>
    public class MovimientoRepositorio : IMovimientoRepositorio
    {
        /// <summary>
        /// Contexto de base de datos.
        /// </summary>
        private readonly EjercicioTecnicoDBContext _dbContext;

        /// <summary>
        /// Constructor que inicializa el repositorio con el contexto de la base de datos.
        /// </summary>
        /// <param name="dbContext">Contexto de la base de datos.</param>
        public MovimientoRepositorio(EjercicioTecnicoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

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

    }
}
