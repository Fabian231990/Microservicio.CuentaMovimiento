using Microservicio.CuentaMovimiento.Dominio.Entidades;
using Microservicio.CuentaMovimiento.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.CuentaMovimiento.Infraestructura.Repositorios
{
    /// <summary>
    /// Implementación del repositorio para la entidad Cuenta.
    /// </summary>
    /// <remarks>
    /// Constructor del repositorio de cuentas.
    /// </remarks>
    /// <param name="dbContext">Contexto de la base de datos.</param>
    public class CuentaRepositorio(EjercicioTecnicoDBContext ejercicioTecnicoDBContext) : ICuentaRepositorio
    {
        private readonly EjercicioTecnicoDBContext ejercicioTecnicoDBContext = ejercicioTecnicoDBContext;

        /// <summary>
        /// Obtiene todas las cuentas de la base de datos.
        /// </summary>
        /// <returns>Listado de cuentas.</returns>
        public async Task<IEnumerable<CuentaEntidad>> ObtenerTodasAsync()
        {
            return await ejercicioTecnicoDBContext.Cuenta.ToListAsync();
        }

        /// <summary>
        /// Obtiene una cuenta por su ID.
        /// </summary>
        /// <param name="numeroCuenta">Numero de la cuenta.</param>
        /// <returns>Entidad Cuenta encontrada.</returns>
        public async Task<CuentaEntidad> ObtenerPorNumeroCuentaAsync(string numeroCuenta)
        {
            return await ejercicioTecnicoDBContext.Cuenta.FirstOrDefaultAsync(filtro => filtro.NumeroCuenta == numeroCuenta);
        }

        /// <summary>
        /// Cuenta el número de cuentas asociadas a un cliente específico.
        /// </summary>
        /// <param name="idCliente">ID del cliente.</param>
        /// <returns>Número de cuentas asociadas al cliente.</returns>
        public async Task<int> ContarCuentasPorClienteAsync(int idCliente)
        {
            return await ejercicioTecnicoDBContext.Cuenta.CountAsync(c => c.IdCliente == idCliente);
        }

        /// <summary>
        /// Obtiene una cuenta asociada a un cliente específico y un tipo de cuenta específico.
        /// </summary>
        /// <param name="idCliente">ID del cliente.</param>
        /// <param name="tipoCuenta">Tipo de cuenta (Ahorro, Corriente, etc.).</param>
        /// <returns>CuentaEntidad correspondiente o null si no se encuentra ninguna.</returns>
        public async Task<CuentaEntidad> ObtenerPorClienteYTipoCuentaAsync(int idCliente, string tipoCuenta)
        {
            return await ejercicioTecnicoDBContext.Cuenta.FirstOrDefaultAsync(c => c.IdCliente == idCliente && c.TipoCuenta.ToLower() == tipoCuenta.ToLower());
        }

        /// <summary>
        /// Crea una nueva cuenta en la base de datos.
        /// </summary>
        /// <param name="cuentaEntidad">Entidad Cuenta a crear.</param>
        public async Task NuevoAsync(CuentaEntidad cuentaEntidad)
        {
            await ejercicioTecnicoDBContext.Cuenta.AddAsync(cuentaEntidad);
            await ejercicioTecnicoDBContext.SaveChangesAsync();
        }

        /// <summary>
        /// Modifica una cuenta existente en la base de datos.
        /// </summary>
        /// <param name="cuentaEntidad">Entidad Cuenta con los datos actualizados.</param>
        public async Task ModificarAsync(CuentaEntidad cuentaEntidad)
        {
            CuentaEntidad cuenta = await ejercicioTecnicoDBContext.Cuenta.FirstOrDefaultAsync(filtro => filtro.IdCuenta == cuentaEntidad.IdCuenta);

            if (cuenta is not null)
            {
                cuenta.SaldoInicial = cuentaEntidad.SaldoInicial;
                cuenta.Estado = cuentaEntidad.Estado;

                await ejercicioTecnicoDBContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Elimina una cuenta de la base de datos por su ID.
        /// </summary>
        /// <param name="idCuenta">Identificador de la cuenta a eliminar.</param>
        public async Task EliminarAsync(int idCuenta)
        {
            CuentaEntidad cuenta = await ejercicioTecnicoDBContext.Cuenta.FindAsync(idCuenta);
            if (cuenta != null)
            {
                ejercicioTecnicoDBContext.Cuenta.Remove(cuenta);
                await ejercicioTecnicoDBContext.SaveChangesAsync();
            }
        }
    }
}
