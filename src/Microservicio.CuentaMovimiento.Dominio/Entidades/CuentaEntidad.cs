using System.ComponentModel.DataAnnotations;

namespace Microservicio.CuentaMovimiento.Dominio.Entidades
{
    /// <summary>
    /// Representa una cuenta bancaria en el sistema.
    /// </summary>
    public class CuentaEntidad
    {
        /// <summary>
        /// Identificador único de la cuenta.
        /// </summary>
        [Key]
        public int IdCuenta { get; set; }

        /// <summary>
        /// Número de la cuenta bancaria.
        /// </summary>
        public string NumeroCuenta { get; set; }

        /// <summary>
        /// Tipo de la cuenta (Ahorro, Corriente, etc.).
        /// </summary>
        public string TipoCuenta { get; set; }

        /// <summary>
        /// Saldo inicial de la cuenta.
        /// </summary>
        public decimal SaldoInicial { get; set; }

        /// <summary>
        /// Estado de la cuenta (activa/inactiva).
        /// </summary>
        public bool Estado { get; set; }

        /// <summary>
        /// Identificador del cliente asociado a la cuenta.
        /// </summary>
        public int IdCliente { get; set; }  // Relación con la entidad Cliente
    }
}
