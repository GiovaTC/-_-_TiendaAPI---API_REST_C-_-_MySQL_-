using Microsoft.EntityFrameworkCore;
using TiendaAPI.Modelos;

namespace TiendaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
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

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(12, 2);

            modelBuilder.Entity<DetallePedido>()
                .Property(d => d.Precio)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Pedidos)
                .WithOne(p => p.Cliente)
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Productos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Detalles)
                .WithOne(d => d.Pedido)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Producto>()
                .HasMany(p => p.Detalles)
                .WithOne(d => d.Producto)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
        }   
    }
}
