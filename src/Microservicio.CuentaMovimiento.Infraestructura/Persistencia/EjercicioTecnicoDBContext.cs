using Microservicio.CuentaMovimiento.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Microservicio.CuentaMovimiento.Infraestructura.Persistencia
{
    /// <summary>
    /// Contexto de la base de datos que maneja las entidades del dominio y su configuracion en la base de datos.
    /// </summary>
    /// <remarks>
    /// Constructor de la clase que inicializa el contexto con las opciones configuradas.
    /// </remarks>
    /// <param name="options">Opciones para configurar el contexto de la base de datos.</param>
    public partial class EjercicioTecnicoDBContext(DbContextOptions<EjercicioTecnicoDBContext> options) : DbContext(options)
    {
        /// <summary>
        /// DbSet que representa la tabla de Clientes en la base de datos.
        /// </summary>
        public virtual DbSet<ClienteEntidad> Clientes { get; set; }

        /// <summary>
        /// DbSet que representa la tabla de Cuentas en la base de datos.
        /// </summary>
        public virtual DbSet<CuentaEntidad> Cuenta { get; set; }

        /// <summary>
        /// DbSet que representa la tabla de Movimientos en la base de datos.
        /// </summary>
        public virtual DbSet<MovimientoEntidad> Movimientos { get; set; }

        /// <summary>
        /// DbSet que representa la tabla de Personas en la base de datos.
        /// </summary>
        public virtual DbSet<PersonaEntidad> Personas { get; set; }

        /// <summary>
        /// Configura el modelo de entidad utilizando el ModelBuilder.
        /// Define las relaciones, claves y restricciones de las entidades.
        /// </summary>
        /// <param name="modelBuilder">Instancia del ModelBuilder utilizada para configurar las entidades.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuracion de la entidad Cliente
            modelBuilder.Entity<ClienteEntidad>(entity =>
            {
                // Definir la clave primaria
                entity.HasKey(e => e.IdCliente).HasName("PK__Cliente__D594664274F8C7AC");

                entity.ToTable("Cliente");

                // Definir un indice unico para IdPersona
                entity.HasIndex(e => e.IdPersona, "UQ_Cliente_IdPersona").IsUnique();

                // Definir las propiedades de la entidad
                entity.Property(e => e.Contrasenia)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                // Definir la relacion con la entidad Persona
                entity.HasOne(d => d.Persona).WithOne(p => p.Cliente)
                    .HasForeignKey<ClienteEntidad>(d => d.IdPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cliente_Persona");
            });

            // Configuracion de la entidad Cuenta
            modelBuilder.Entity<CuentaEntidad>(entity =>
            {
                // Definir la clave primaria
                entity.HasKey(e => e.IdCuenta).HasName("PK__Cuenta__D41FD70696CDD0C1");

                // Definir un indice unico para NumeroCuenta
                entity.HasIndex(e => e.NumeroCuenta, "UQ__Cuenta__E039507B69E525CF").IsUnique();

                // Definir las propiedades de la entidad
                entity.Property(e => e.NumeroCuenta)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.SaldoInicial).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TipoCuenta)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                // Definir la relacion con la entidad Cliente
                entity.HasOne(d => d.Cliente).WithMany(p => p.Cuenta)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cuenta_Cliente");
            });

            // Configuracion de la entidad Movimiento
            modelBuilder.Entity<MovimientoEntidad>(entity =>
            {
                // Definir la clave primaria
                entity.HasKey(e => e.IdMovimiento).HasName("PK__Movimien__881A6AE0D6B82CB5");

                entity.ToTable("Movimiento");

                // Definir las propiedades de la entidad
                entity.Property(e => e.Fecha).HasColumnType("datetime");
                entity.Property(e => e.Saldo).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.TipoMovimiento)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Valor).HasColumnType("decimal(18, 2)");

                // Definir la relacion con la entidad Cuenta
                entity.HasOne(d => d.Cuenta).WithMany(p => p.Movimientos)
                    .HasForeignKey(d => d.IdCuenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Movimiento_Cuenta");
            });

            // Configuracion de la entidad Persona
            modelBuilder.Entity<PersonaEntidad>(entity =>
            {
                // Definir la clave primaria
                entity.HasKey(e => e.IdPersona).HasName("PK__Persona__2EC8D2ACF128603A");

                entity.ToTable("Persona");

                // Definir un indice unico para Identificacion
                entity.HasIndex(e => e.Identificacion, "UQ__Persona__D6F931E5FB75B7B9").IsUnique();

                // Definir las propiedades de la entidad
                entity.Property(e => e.Direccion)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Genero)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.Identificacion)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Telefono)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            // Metodo parcial para configuraciones adicionales
            OnModelCreatingPartial(modelBuilder);
        }

        /// <summary>
        /// Metodo parcial para permitir configuraciones adicionales en la creacion del modelo.
        /// </summary>
        /// <param name="modelBuilder">El generador del modelo de entidad.</param>
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
