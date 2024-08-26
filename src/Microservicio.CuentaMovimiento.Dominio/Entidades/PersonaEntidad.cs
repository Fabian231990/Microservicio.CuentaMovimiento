namespace Microservicio.CuentaMovimiento.Dominio.Entidades
{
    /// <summary>
    /// Representa la entidad Persona en el dominio, con informacion basica personal y asociacion a un cliente.
    /// </summary>
    public partial class PersonaEntidad
    {
        /// <summary>
        /// Identificador unico de la persona.
        /// </summary>
        public int IdPersona { get; set; }

        /// <summary>
        /// Nombre completo de la persona.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Genero de la persona (ejemplo: Masculino, Femenino).
        /// </summary>
        public string Genero { get; set; }

        /// <summary>
        /// Edad de la persona.
        /// </summary>
        public int Edad { get; set; }

        /// <summary>
        /// Numero de identificacion unico de la persona.
        /// </summary>
        public string Identificacion { get; set; }

        /// <summary>
        /// Direccion de residencia de la persona.
        /// </summary>
        public string Direccion { get; set; }

        /// <summary>
        /// Numero de telefono de la persona.
        /// </summary>
        public string Telefono { get; set; }

        /// <summary>
        /// Navegacion hacia la entidad Cliente asociada con esta persona.
        /// </summary>
        public virtual ClienteEntidad Cliente { get; set; }
    }
}
