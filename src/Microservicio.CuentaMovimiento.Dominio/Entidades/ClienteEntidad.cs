namespace Microservicio.CuentaMovimiento.Dominio.Entidades
{
    /// <summary>
    /// Representa la entidad Cliente en el dominio, asociada a una Persona y con una o mas cuentas.
    /// </summary>
    public partial class ClienteEntidad
    {
        /// <summary>
        /// Identificador unico del cliente.
        /// </summary>
        public int IdCliente { get; set; }

        /// <summary>
        /// Identificador unico de la persona asociada al cliente.
        /// </summary>
        public int IdPersona { get; set; }

        /// <summary>
        /// Contraseña del cliente para acceder a sus servicios.
        /// </summary>
        public string Contrasenia { get; set; }

        /// <summary>
        /// Estado del cliente. Indica si el cliente esta activo o inactivo.
        /// </summary>
        public bool Estado { get; set; }

        /// <summary>
        /// Coleccion de cuentas asociadas al cliente.
        /// </summary>
        public virtual ICollection<CuentaEntidad> Cuenta { get; set; } = [];

        /// <summary>
        /// Entidad Persona asociada al cliente.
        /// </summary>
        public virtual PersonaEntidad Persona { get; set; }
    }
}
