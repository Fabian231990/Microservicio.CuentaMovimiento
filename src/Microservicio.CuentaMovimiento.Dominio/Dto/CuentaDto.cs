namespace Microservicio.CuentaMovimiento.Dominio.Dto
{
    /// <summary>
    /// Data Transfer Object (DTO) para la entidad Cuenta.
    /// </summary>
    public class CuentaDto
    {
        /// <summary>
        /// Identificador unico de la cuenta.
        /// </summary>
        public int IdCuenta { get; set; }

        /// <summary>
        /// Numero de cuenta unico asociado a la cuenta bancaria.
        /// </summary>
        public string NumeroCuenta { get; set; }

        /// <summary>
        /// Tipo de cuenta, como "Ahorro" o "Corriente".
        /// </summary>
        public string TipoCuenta { get; set; }

        /// <summary>
        /// Saldo inicial de la cuenta al momento de su creacion.
        /// </summary>
        public decimal SaldoInicial { get; set; }

        /// <summary>
        /// Estado de la cuenta, indicando si esta activa o inactiva.
        /// </summary>
        public bool Estado { get; set; }

        /// <summary>
        /// Identificador unico del cliente asociado a esta cuenta.
        /// </summary>
        public int IdCliente { get; set; }

        /// <summary>
        /// Nombre del cliente asociado a la cuenta.
        /// </summary>
        public string NombreCliente { get; set; }

        /// <summary>
        /// Identificacion del cliente asociado a la cuenta.
        /// </summary>
        public string Identificacion { get; set; }
    }
}
