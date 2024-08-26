using Microservicio.CuentaMovimiento.Dominio.Dto;
using Microservicio.CuentaMovimiento.Dominio.Entidades;
using Microservicio.CuentaMovimiento.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.CuentaMovimiento.Infraestructura.Repositorios
{
    /// <summary>
    /// Repositorio de la entidad Cuenta, implementa las operaciones CRUD.
    /// </summary>
    /// <remarks>
    /// Constructor del repositorio de cuentas.
    /// </remarks>
    /// <param name="dbContext">Contexto de base de datos.</param>
    public class CuentaRepositorio(EjercicioTecnicoDBContext dbContext) : ICuentaRepositorio
    {
        /// <summary>
        /// Contexto de base de datos.
        /// </summary>
        private readonly EjercicioTecnicoDBContext _dbContext = dbContext;

        /// <summary>
        /// Obtiene todas las cuentas registradas en la base de datos.
        /// </summary>
        /// <returns>Una lista con todas las cuentas registradas en formato DTO.</returns>
        public async Task<IEnumerable<CuentaDto>> ObtenerTodasAsync()
        {
            var cuentas = await _dbContext.Cuenta
                .Include(c => c.Cliente)
                .ThenInclude(cliente => cliente.Persona)
                .ToListAsync();

            return cuentas.Select(c => new CuentaDto
            {
                IdCuenta = c.IdCuenta,
                NumeroCuenta = c.NumeroCuenta,
                TipoCuenta = c.TipoCuenta,
                SaldoInicial = c.SaldoInicial,
                Estado = c.Estado,
                Identificacion = c.Cliente.Persona.Identificacion,
                IdCliente = c.IdCliente,
                NombreCliente = c.Cliente.Persona.Nombre
            }).ToList();
        }

        /// <summary>
        /// Obtiene una cuenta por su numero de cuenta.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta a buscar.</param>
        /// <returns>La cuenta encontrada en formato DTO o null si no existe.</returns>
        public async Task<CuentaDto> ObtenerPorNumeroCuentaAsync(string numeroCuenta)
        {
            var cuenta = await _dbContext.Cuenta
                .Include(c => c.Cliente)
                .ThenInclude(cliente => cliente.Persona)
                .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);

            if (cuenta == null) return null;

            return new CuentaDto
            {
                IdCuenta = cuenta.IdCuenta,
                NumeroCuenta = cuenta.NumeroCuenta,
                TipoCuenta = cuenta.TipoCuenta,
                SaldoInicial = cuenta.SaldoInicial,
                Estado = cuenta.Estado,
                Identificacion = cuenta.Cliente.Persona.Identificacion,
                IdCliente = cuenta.IdCliente,
                NombreCliente = cuenta.Cliente.Persona.Nombre
            };
        }

        /// <summary>
        /// Crea una nueva cuenta en la base de datos.
        /// </summary>
        /// <param name="cuentaDto">DTO de la cuenta que se va a crear.</param>
        /// <returns>La cuenta creada en formato DTO.</returns>
        public async Task<CuentaDto> NuevoAsync(CuentaDto cuentaDto)
        {
            var cliente = await _dbContext.Clientes
                .Include(c => c.Persona)
                .FirstOrDefaultAsync(c => c.Persona.Identificacion == cuentaDto.Identificacion);

            if (cliente == null) return null;

            var cuentaEntidad = new CuentaEntidad
            {
                NumeroCuenta = cuentaDto.NumeroCuenta,
                TipoCuenta = cuentaDto.TipoCuenta,
                SaldoInicial = cuentaDto.SaldoInicial,
                Estado = cuentaDto.Estado,
                IdCliente = cliente.IdCliente
            };

            _dbContext.Cuenta.Add(cuentaEntidad);
            await _dbContext.SaveChangesAsync();

            cuentaDto.IdCuenta = cuentaEntidad.IdCuenta;
            cuentaDto.NombreCliente = cliente.Persona.Nombre;
            cuentaDto.IdCliente = cliente.IdCliente;
            return cuentaDto;
        }

        /// <summary>
        /// Modifica los datos de una cuenta existente en la base de datos.
        /// </summary>
        /// <param name="cuentaDto">DTO de la cuenta con los datos actualizados.</param>
        /// <returns>La cuenta actualizada en formato DTO o null si no se encuentra.</returns>
        public async Task<CuentaDto> ModificarAsync(CuentaDto cuentaDto)
        {
            var cuenta = await _dbContext.Cuenta
                .Include(c => c.Cliente)
                .ThenInclude(cliente => cliente.Persona)
                .FirstOrDefaultAsync(c => c.NumeroCuenta == cuentaDto.NumeroCuenta);

            if (cuenta == null) return null;

            cuenta.SaldoInicial = cuentaDto.SaldoInicial;
            cuenta.Estado = cuentaDto.Estado;

            await _dbContext.SaveChangesAsync();

            return cuentaDto;
        }

        /// <summary>
        /// Elimina una cuenta de la base de datos por su numero de cuenta.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta a eliminar.</param>
        /// <returns>True si la cuenta fue eliminada exitosamente, de lo contrario false.</returns>
        public async Task<bool> EliminarAsync(string numeroCuenta)
        {
            var cuenta = await _dbContext.Cuenta
                .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);

            if (cuenta == null) return false;

            _dbContext.Cuenta.Remove(cuenta);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Cuenta cuantas cuentas tiene un cliente basado en la identificacion de la persona.
        /// </summary>
        /// <param name="identificacion">La identificacion de la persona.</param>
        /// <returns>El numero de cuentas asociadas a la persona.</returns>
        public async Task<int> ContarCuentasPorIdentificacionAsync(string identificacion)
        {
            return await _dbContext.Cuenta
                .CountAsync(cuenta => cuenta.Cliente.Persona.Identificacion == identificacion);
        }

        /// <summary>
        /// Obtiene todas las cuentas asociadas a un cliente por su identificacion.
        /// </summary>
        /// <param name="identificacion">Identificacion del cliente.</param>
        /// <returns>Listado de cuentas asociadas a la identificacion del cliente.</returns>
        public async Task<IEnumerable<CuentaDto>> ObtenerCuentasPorIdentificacionAsync(string identificacion)
        {
            var cuentas = await _dbContext.Cuenta
                .Include(c => c.Cliente)
                .ThenInclude(cliente => cliente.Persona)
                .Where(c => c.Cliente.Persona.Identificacion == identificacion)
                .ToListAsync();

            return cuentas.Select(c => new CuentaDto
            {
                IdCuenta = c.IdCuenta,
                NumeroCuenta = c.NumeroCuenta,
                TipoCuenta = c.TipoCuenta,
                SaldoInicial = c.SaldoInicial,
                Estado = c.Estado,
                IdCliente = c.IdCliente,
                NombreCliente = c.Cliente.Persona.Nombre,
                Identificacion = c.Cliente.Persona.Identificacion
            });
        }
    }
}
