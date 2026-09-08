using Microsoft.EntityFrameworkCore;
using TiendaAPI.Modelos;
using TiendaAPI.Modelos;

namespace TiendaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<DetallePedido> DetallesPedido => Set<DetallePedido>();
        public DbSet<Categoria> Categorias => Set<Categoria>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =====================================================
            // TABLAS
            // =====================================================

            modelBuilder.Entity<Cliente>()
                .ToTable("cliente");

            modelBuilder.Entity<Pedido>()
                .ToTable("pedido");

            modelBuilder.Entity<Producto>()
                .ToTable("producto");

            modelBuilder.Entity<DetallePedido>()
                .ToTable("detalle_pedido");

            modelBuilder.Entity<Categoria>()
                .ToTable("categoria");


            // =====================================================
            // CLIENTE
            // =====================================================

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Nombre)
                .HasColumnName("nombre");

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Correo)
                .HasColumnName("correo");

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Telefono)
                .HasColumnName("telefono");


            // =====================================================
            // PEDIDO
            // =====================================================

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Fecha)
                .HasColumnName("fecha");

            modelBuilder.Entity<Pedido>()
                .Property(p => p.ClienteId)
                .HasColumnName("cliente_id");


            // =====================================================
            // CATEGORIA
            // =====================================================

            modelBuilder.Entity<Categoria>()
                .Property(c => c.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Categoria>()
                .Property(c => c.Nombre)
                .HasColumnName("nombre");


            // =====================================================
            // PRODUCTO
            // =====================================================

            modelBuilder.Entity<Producto>()
                .Property(p => p.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Producto>()
                .Property(p => p.Nombre)
                .HasColumnName("nombre");

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasColumnName("precio")
                .HasPrecision(12, 2);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Stock)
                .HasColumnName("stock");

            modelBuilder.Entity<Producto>()
                .Property(p => p.CategoriaId)
                .HasColumnName("categoria_id");


            // =====================================================
            // DETALLE PEDIDO
            // =====================================================

            modelBuilder.Entity<DetallePedido>()
                .Property(d => d.Id)
                .HasColumnName("id");

            modelBuilder.Entity<DetallePedido>()
                .Property(d => d.Cantidad)
                .HasColumnName("cantidad");

            modelBuilder.Entity<DetallePedido>()
                .Property(d => d.Precio)
                .HasColumnName("precio")
                .HasPrecision(12, 2);

            modelBuilder.Entity<DetallePedido>()
                .Property(d => d.PedidoId)
                .HasColumnName("pedido_id");

            modelBuilder.Entity<DetallePedido>()
                .Property(d => d.ProductoId)
                .HasColumnName("producto_id");


            // =====================================================
            // RELACIÓN CLIENTE -> PEDIDOS
            // =====================================================

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Pedidos)
                .WithOne(p => p.Cliente)
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // RELACIÓN CATEGORIA -> PRODUCTOS
            // =====================================================

            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Productos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // RELACIÓN PEDIDO -> DETALLES
            // =====================================================

            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Detalles)
                .WithOne(d => d.Pedido)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // RELACIÓN PRODUCTO -> DETALLES
            // =====================================================

            modelBuilder.Entity<Producto>()
                .HasMany(p => p.Detalles)
                .WithOne(d => d.Producto)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}   