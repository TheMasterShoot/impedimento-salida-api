using System;
using System.Collections.Generic;
using impedimento_salidaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace impedimento_salidaAPI.Context;

public partial class ImpedimentoSalidaContext : DbContext
{
    public ImpedimentoSalidaContext()
    {
    }

    public ImpedimentoSalidaContext(DbContextOptions<ImpedimentoSalidaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ca> Cas { get; set; }

    public virtual DbSet<CertificacionExistencium> CertificacionExistencia { get; set; }

    public virtual DbSet<Ciudadano> Ciudadanos { get; set; }

    public virtual DbSet<Estatus> Estatuses { get; set; }

    public virtual DbSet<Rechazo> Rechazos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SolicitudLevantamiento> SolicitudLevantamientos { get; set; }

    public virtual DbSet<TipoEstatus> TipoEstatuses { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=impedimento-salida;Trusted_Connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ca>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CAS__3214EC27A84640C8");

            entity.ToTable("CAS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cas)
                .HasMaxLength(15)
                .HasColumnName("CAS");
            entity.Property(e => e.Cedula)
                .HasMaxLength(11)
                .HasColumnName("CEDULA");
            entity.Property(e => e.Certificacionid).HasColumnName("CERTIFICACIONID");

            entity.HasOne(d => d.Certificacion).WithMany(p => p.CasNavigation)
                .HasForeignKey(d => d.Certificacionid)
                .HasConstraintName("FK_CAS_CERTIFICACIONID");
        });

        modelBuilder.Entity<CertificacionExistencium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CERTIFIC__3214EC2713143A09");

            entity.ToTable("CERTIFICACION_EXISTENCIA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("APELLIDO");
            entity.Property(e => e.Carta)
                .HasColumnType("text")
                .HasColumnName("CARTA");
            entity.Property(e => e.Cas)
                .HasMaxLength(15)
                .HasColumnName("CAS");
            entity.Property(e => e.Cedula)
                .HasMaxLength(11)
                .HasColumnName("CEDULA");
            entity.Property(e => e.Ciudadanoid).HasColumnName("CIUDADANOID");
            entity.Property(e => e.Estatusid).HasColumnName("ESTATUSID");
            entity.Property(e => e.FechaAprobacion).HasColumnName("FECHA_APROBACION");
            entity.Property(e => e.FechaSolicitud).HasColumnName("FECHA_SOLICITUD");
            entity.Property(e => e.Impedimento)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("IMPEDIMENTO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Reporte)
                .HasColumnType("text")
                .HasColumnName("REPORTE");
            entity.Property(e => e.UsuarioAprobacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USUARIO_APROBACION");

            entity.HasOne(d => d.Ciudadano).WithMany(p => p.CertificacionExistencia)
                .HasForeignKey(d => d.Ciudadanoid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CERTIFICACION_EXISTENCIA_CIUDADANOID");

            entity.HasOne(d => d.Estatus).WithMany(p => p.CertificacionExistencia)
                .HasForeignKey(d => d.Estatusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CERTIFICACION_EXISTENCIA_ESTATUSID");
        });

        modelBuilder.Entity<Ciudadano>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CIUDADAN__3214EC27FC79657E");

            entity.ToTable("CIUDADANO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("APELLIDO");
            entity.Property(e => e.Cedula)
                .HasMaxLength(11)
                .HasColumnName("CEDULA");
            entity.Property(e => e.Email)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Password)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Rolid).HasColumnName("ROLID");
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .HasColumnName("TELEFONO");

            entity.HasOne(d => d.Rol).WithMany(p => p.Ciudadanos)
                .HasForeignKey(d => d.Rolid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CIUDADANO_ROL_ID");
        });

        modelBuilder.Entity<Estatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ESTATUS__3214EC2792FBA020");

            entity.ToTable("ESTATUS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Codigo)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.TipoCodigo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("TIPO_CODIGO");

            entity.HasOne(d => d.TipoCodigoNavigation).WithMany(p => p.Estatuses)
                .HasForeignKey(d => d.TipoCodigo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ESTATUS_TIPO_CODIGO");
        });

        modelBuilder.Entity<Rechazo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RECHAZO__3214EC27DC62557F");

            entity.ToTable("RECHAZO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Certificacionid).HasColumnName("CERTIFICACIONID");
            entity.Property(e => e.Levantamientoid).HasColumnName("LEVANTAMIENTOID");
            entity.Property(e => e.Motivo)
                .HasColumnType("text")
                .HasColumnName("MOTIVO");

            entity.HasOne(d => d.Certificacion).WithMany(p => p.Rechazos)
                .HasForeignKey(d => d.Certificacionid)
                .HasConstraintName("FK_RECHAZO_CERTIFICACIONID");

            entity.HasOne(d => d.Levantamiento).WithMany(p => p.Rechazos)
                .HasForeignKey(d => d.Levantamientoid)
                .HasConstraintName("FK_RECHAZO_LEVANTAMIENTOID");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ROLES__3214EC276FD885C0");

            entity.ToTable("ROLES");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ROL");
        });

        modelBuilder.Entity<SolicitudLevantamiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SOLICITU__3214EC27BEE0E160");

            entity.ToTable("SOLICITUD_LEVANTAMIENTO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("APELLIDO");
            entity.Property(e => e.Carta).HasColumnName("CARTA");
            entity.Property(e => e.Cedula)
                .HasMaxLength(11)
                .HasColumnName("CEDULA");
            entity.Property(e => e.Ciudadanoid).HasColumnName("CIUDADANOID");
            entity.Property(e => e.Estatusid).HasColumnName("ESTATUSID");
            entity.Property(e => e.FechaAprobacion).HasColumnName("FECHA_APROBACION");
            entity.Property(e => e.FechaSolicitud).HasColumnName("FECHA_SOLICITUD");
            entity.Property(e => e.LevantamientoTipo)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("LEVANTAMIENTO_TIPO");
            entity.Property(e => e.NoRecurso).HasColumnName("NO_RECURSO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Reporte)
                .HasColumnType("text")
                .HasColumnName("REPORTE");
            entity.Property(e => e.Sentencia).HasColumnName("SENTENCIA");
            entity.Property(e => e.UsuarioAprobacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USUARIO_APROBACION");

            entity.HasOne(d => d.Ciudadano).WithMany(p => p.SolicitudLevantamientos)
                .HasForeignKey(d => d.Ciudadanoid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SOLICITUD_LEVANTAMIENTO_CIUDADANOID");

            entity.HasOne(d => d.Estatus).WithMany(p => p.SolicitudLevantamientos)
                .HasForeignKey(d => d.Estatusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SOLICITUD_LEVANTAMIENTO_ESTATUSID");
        });

        modelBuilder.Entity<TipoEstatus>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("TIPO_ESTATUS");

            entity.Property(e => e.Codigo)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("CODIGO");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USUARIO__3214EC27495105A8");

            entity.ToTable("USUARIO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("APELLIDO");
            entity.Property(e => e.Estatusid).HasColumnName("ESTATUSID");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Password)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Rolid).HasColumnName("ROLID");
            entity.Property(e => e.Username)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.Estatus).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Estatusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USUARIO_ESTATUS_ID");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Rolid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USUARIO_ROL_ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
