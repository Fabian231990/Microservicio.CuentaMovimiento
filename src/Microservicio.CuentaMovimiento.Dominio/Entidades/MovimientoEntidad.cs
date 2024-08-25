using System.ComponentModel.DataAnnotations;

namespace Microservicio.CuentaMovimiento.Dominio.Entidades
{
    /// <summary>
    /// Representa un movimiento financiero asociado a una cuenta.
    /// </summary>
    public class MovimientoEntidad
    {
        /// <summary>
        /// Identificador único del movimiento.
        /// </summary>
        [Key]
        public int IdMovimiento { get; set; }

        /// <summary>
        /// Fecha y hora en que se realizó el movimiento.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Tipo de movimiento, por ejemplo: "Depósito", "Retiro".
        /// </summary>
        public string TipoMovimiento { get; set; }

        /// <summary>
        /// Valor monetario del movimiento.
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Saldo de la cuenta después de realizar el movimiento.
        /// </summary>
        public decimal Saldo { get; set; }

        /// <summary>
        /// Identificador de la cuenta asociada al movimiento.
        /// </summary>
        public int IdCuenta { get; set; }
    }
}
