# 🛒 TiendaAPI --- API REST C# + MySQL

<img width="1254" height="1254" alt="image" src="https://github.com/user-attachments/assets/374aa37c-9b58-4946-91cf-1dfa2078c401" />      

```

API REST desarrollada con **Visual Studio 2026, C#, ASP.NET Core Web
API, .NET 10, Entity Framework Core y MySQL**.

El proyecto permite administrar una tienda mediante endpoints REST y
probar todas las operaciones desde **Postman**.

------------------------------------------------------------------------

## 📌 Características

-   API REST con ASP.NET Core.
-   .NET 10.
-   Entity Framework Core.
-   MySQL / MariaDB.
-   Operaciones CRUD.
-   Relaciones entre entidades.
-   JSON anidado.
-   Validaciones básicas.
-   Swagger/OpenAPI.
-   Compatible con Postman.
-   Script SQL para crear la base de datos.
-   Datos iniciales para pruebas.

------------------------------------------------------------------------

# 🧩 Modelo de datos

El ejemplo utiliza **2 elementos padre y 3 elementos hijos**.

## Elementos padre

1.  `Cliente`
2.  `Pedido`

## Elementos hijo

1.  `Producto`
2.  `DetallePedido`
3.  `Categoria`

Relación:

``` text
CLIENTE
   │
   └── PEDIDO
          │
          └── DETALLE_PEDIDO
                    │
                    └── PRODUCTO
                           │
                           └── CATEGORIA
```

------------------------------------------------------------------------

# 📁 Estructura del proyecto

``` text
TiendaAPI/
│
├── Controllers/
│   ├── ClientesController.cs
│   ├── PedidosController.cs
│   ├── ProductosController.cs
│   ├── DetallesPedidoController.cs
│   └── CategoriasController.cs
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── Models/
│   ├── Cliente.cs
│   ├── Pedido.cs
│   ├── Producto.cs
│   ├── DetallePedido.cs
│   └── Categoria.cs
│
├── Program.cs
├── appsettings.json
└── TiendaAPI.csproj
```

------------------------------------------------------------------------

# 1. Crear el proyecto

En Visual Studio 2026:

``` text
Crear un nuevo proyecto
        ↓
ASP.NET Core Web API
        ↓
Nombre: TiendaAPI
        ↓
Framework: .NET 10
        ↓
Crear
```

También se puede crear mediante terminal:

``` bash
dotnet new webapi -n TiendaAPI
cd TiendaAPI
```

------------------------------------------------------------------------

# 2. Paquetes NuGet

Instalar:

``` bash
dotnet add package Microsoft.EntityFrameworkCore --version 10.0.11
dotnet add package Microsoft.EntityFrameworkCore.Design --version 10.0.11
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 10.0.11
```

> Si la versión estable disponible de Pomelo para tu instalación de .NET
> 10 es diferente, utiliza una versión compatible con EF Core 10.

------------------------------------------------------------------------

# 3. Archivo TiendaAPI.csproj

``` xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.11" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.11">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="10.0.11" />
  </ItemGroup>

</Project>
```

------------------------------------------------------------------------

# 4. Base de datos MySQL

Crear la base de datos:

``` sql
CREATE DATABASE tienda_db
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE tienda_db;
```

------------------------------------------------------------------------

# 5. Script SQL completo

Guardar como:

``` text
tienda_db.sql
```

Contenido:

``` sql
CREATE DATABASE IF NOT EXISTS tienda_db
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE tienda_db;

DROP TABLE IF EXISTS detalle_pedido;
DROP TABLE IF EXISTS pedido;
DROP TABLE IF EXISTS producto;
DROP TABLE IF EXISTS categoria;
DROP TABLE IF EXISTS cliente;

CREATE TABLE cliente (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    correo VARCHAR(150) NOT NULL,
    telefono VARCHAR(30)
);

CREATE TABLE categoria (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL
);

CREATE TABLE producto (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(150) NOT NULL,
    precio DECIMAL(12,2) NOT NULL,
    stock INT NOT NULL DEFAULT 0,
    categoria_id INT NOT NULL,

    CONSTRAINT fk_producto_categoria
        FOREIGN KEY (categoria_id)
        REFERENCES categoria(id)
);

CREATE TABLE pedido (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fecha DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    cliente_id INT NOT NULL,

    CONSTRAINT fk_pedido_cliente
        FOREIGN KEY (cliente_id)
        REFERENCES cliente(id)
);

CREATE TABLE detalle_pedido (
    id INT AUTO_INCREMENT PRIMARY KEY,
    cantidad INT NOT NULL,
    precio DECIMAL(12,2) NOT NULL,
    pedido_id INT NOT NULL,
    producto_id INT NOT NULL,

    CONSTRAINT fk_detalle_pedido
        FOREIGN KEY (pedido_id)
        REFERENCES pedido(id)
        ON DELETE CASCADE,

    CONSTRAINT fk_detalle_producto
        FOREIGN KEY (producto_id)
        REFERENCES producto(id)
);

INSERT INTO cliente (nombre, correo, telefono) VALUES
('Juan Pérez', 'juan@gmail.com', '3001111111'),
('María Gómez', 'maria@gmail.com', '3002222222'),
('Carlos Rodríguez', 'carlos@gmail.com', '3003333333');

INSERT INTO categoria (nombre) VALUES
('Computadores'),
('Accesorios'),
('Monitores');

INSERT INTO producto (nombre, precio, stock, categoria_id) VALUES
('Laptop Lenovo', 2500000, 10, 1),
('Mouse Logitech', 85000, 25, 2),
('Monitor LG 24"', 650000, 8, 3),
('Teclado Logitech', 120000, 15, 2);

INSERT INTO pedido (fecha, cliente_id) VALUES
('2026-09-01 09:30:00', 1),
('2026-09-02 10:15:00', 2),
('2026-09-03 08:45:00', 1);

INSERT INTO detalle_pedido
(cantidad, precio, pedido_id, producto_id)
VALUES
(1, 2500000, 1, 1),
(2, 85000, 1, 2),
(1, 650000, 2, 3),
(1, 120000, 3, 4);
```

------------------------------------------------------------------------

# 6. Modelos

## Models/Cliente.cs

``` csharp
using System.ComponentModel.DataAnnotations;

namespace TiendaAPI.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Correo { get; set; } = string.Empty;

        [StringLength(30)]
        public string? Telefono { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }
            = new List<Pedido>();
    }
}
```

------------------------------------------------------------------------

## Models/Pedido.cs

``` csharp
using System.ComponentModel.DataAnnotations;

namespace TiendaAPI.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        [Required]
        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; }

        public ICollection<DetallePedido> Detalles { get; set; }
            = new List<DetallePedido>();
    }
}
```

------------------------------------------------------------------------

## Models/Producto.cs

``` csharp
using System.ComponentModel.DataAnnotations;

namespace TiendaAPI.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public int CategoriaId { get; set; }

        public Categoria? Categoria { get; set; }

        public ICollection<DetallePedido> Detalles { get; set; }
            = new List<DetallePedido>();
    }
}
```

------------------------------------------------------------------------

## Models/DetallePedido.cs

``` csharp
using System.ComponentModel.DataAnnotations;

namespace TiendaAPI.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Precio { get; set; }

        public int PedidoId { get; set; }

        public Pedido? Pedido { get; set; }

        public int ProductoId { get; set; }

        public Producto? Producto { get; set; }
    }
}
```

------------------------------------------------------------------------

## Models/Categoria.cs

``` csharp
using System.ComponentModel.DataAnnotations;

namespace TiendaAPI.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public ICollection<Producto> Productos { get; set; }
            = new List<Producto>();
    }
}
```

------------------------------------------------------------------------

# 7. ApplicationDbContext

Crear:

``` text
Data/ApplicationDbContext.cs
```

Código:

``` csharp
using Microsoft.EntityFrameworkCore;
using TiendaAPI.Models;

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
```

------------------------------------------------------------------------

# 8. appsettings.json

Configurar la conexión a MySQL:

``` json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=tienda_db;User=root;Password=;"
  },

  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },

  "AllowedHosts": "*"
}
```

Si MySQL tiene contraseña:

``` text
Password=TU_PASSWORD
```

Por ejemplo:

``` json
"DefaultConnection": "Server=localhost;Port=3306;Database=tienda_db;User=root;Password=123456;"
```

------------------------------------------------------------------------

# 9. Program.cs

Reemplazar el contenido por:

``` csharp
using Microsoft.EntityFrameworkCore;
using TiendaAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PermitirTodo");

app.UseAuthorization();

app.MapControllers();

app.Run();
```

------------------------------------------------------------------------

# 10. Controller Clientes

Crear:

``` text
Controllers/ClientesController.cs
```

``` csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaAPI.Data;
using TiendaAPI.Models;

namespace TiendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            var clientes = await _context.Clientes
                .Include(c => c.Pedidos)
                    .ThenInclude(p => p.Detalles)
                        .ThenInclude(d => d.Producto)
                .AsNoTracking()
                .ToListAsync();

            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Pedidos)
                    .ThenInclude(p => p.Detalles)
                        .ThenInclude(d => d.Producto)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
                return NotFound(new
                {
                    mensaje = "Cliente no encontrado"
                });

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(
            Cliente cliente)
        {
            _context.Clientes.Add(cliente);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCliente),
                new { id = cliente.Id },
                cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(
            int id,
            Cliente cliente)
        {
            if (id != cliente.Id)
                return BadRequest();

            var existe = await _context.Clientes
                .AnyAsync(c => c.Id == id);

            if (!existe)
                return NotFound();

            _context.Entry(cliente).State =
                EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes
                .FindAsync(id);

            if (cliente == null)
                return NotFound();

            _context.Clientes.Remove(cliente);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    mensaje =
                        "No se puede eliminar el cliente porque tiene pedidos asociados."
                });
            }

            return NoContent();
        }
    }
}
```

------------------------------------------------------------------------

# 11. Controller Pedidos

Crear:

``` text
Controllers/PedidosController.cs
```

``` csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaAPI.Data;
using TiendaAPI.Models;

namespace TiendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PedidosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                        .ThenInclude(p => p!.Categoria)
                .AsNoTracking()
                .ToListAsync();

            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                        .ThenInclude(p => p!.Categoria)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound(new
                {
                    mensaje = "Pedido no encontrado"
                });

            return Ok(pedido);
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(
            Pedido pedido)
        {
            var clienteExiste = await _context.Clientes
                .AnyAsync(c => c.Id == pedido.ClienteId);

            if (!clienteExiste)
            {
                return BadRequest(new
                {
                    mensaje = "El cliente no existe"
                });
            }

            pedido.Fecha = DateTime.Now;

            _context.Pedidos.Add(pedido);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetPedido),
                new { id = pedido.Id },
                pedido);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            var pedido = await _context.Pedidos
                .FindAsync(id);

            if (pedido == null)
                return NotFound();

            _context.Pedidos.Remove(pedido);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
```

------------------------------------------------------------------------

# 12. Controller Productos

Crear:

``` text
Controllers/ProductosController.cs
```

``` csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaAPI.Data;
using TiendaAPI.Models;

namespace TiendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .AsNoTracking()
                .ToListAsync();

            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
                return NotFound();

            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(
            Producto producto)
        {
            var categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Id == producto.CategoriaId);

            if (!categoriaExiste)
            {
                return BadRequest(new
                {
                    mensaje = "La categoría no existe"
                });
            }

            _context.Productos.Add(producto);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProducto),
                new { id = producto.Id },
                producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(
            int id,
            Producto producto)
        {
            if (id != producto.Id)
                return BadRequest();

            if (!await _context.Categorias
                .AnyAsync(c => c.Id == producto.CategoriaId))
            {
                return BadRequest(new
                {
                    mensaje = "La categoría no existe"
                });
            }

            _context.Entry(producto).State =
                EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos
                .FindAsync(id);

            if (producto == null)
                return NotFound();

            _context.Productos.Remove(producto);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    mensaje =
                        "No se puede eliminar el producto porque tiene detalles de pedidos."
                });
            }

            return NoContent();
        }
    }
}
```

------------------------------------------------------------------------

# 13. Controller Categorias

Crear:

``` text
Controllers/CategoriasController.cs
```

``` csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaAPI.Data;
using TiendaAPI.Models;

namespace TiendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            var categorias = await _context.Categorias
                .Include(c => c.Productos)
                .AsNoTracking()
                .ToListAsync();

            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias
                .Include(c => c.Productos)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
                return NotFound();

            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> PostCategoria(
            Categoria categoria)
        {
            _context.Categorias.Add(categoria);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCategoria),
                new { id = categoria.Id },
                categoria);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(
            int id,
            Categoria categoria)
        {
            if (id != categoria.Id)
                return BadRequest();

            _context.Entry(categoria).State =
                EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias
                .FindAsync(id);

            if (categoria == null)
                return NotFound();

            _context.Categorias.Remove(categoria);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    mensaje =
                        "No se puede eliminar la categoría porque tiene productos asociados."
                });
            }

            return NoContent();
        }
    }
}
```

------------------------------------------------------------------------

# 14. Controller DetallesPedido

Crear:

``` text
Controllers/DetallesPedidoController.cs
```

``` csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaAPI.Data;
using TiendaAPI.Models;

namespace TiendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetallesPedidoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DetallesPedidoController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetallePedido>>>
            GetDetalles()
        {
            var detalles = await _context.DetallesPedido
                .Include(d => d.Pedido)
                .Include(d => d.Producto)
                .AsNoTracking()
                .ToListAsync();

            return Ok(detalles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetallePedido>>
            GetDetalle(int id)
        {
            var detalle = await _context.DetallesPedido
                .Include(d => d.Pedido)
                .Include(d => d.Producto)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);

            if (detalle == null)
                return NotFound();

            return Ok(detalle);
        }

        [HttpPost]
        public async Task<ActionResult<DetallePedido>>
            PostDetalle(DetallePedido detalle)
        {
            var pedidoExiste = await _context.Pedidos
                .AnyAsync(p => p.Id == detalle.PedidoId);

            var productoExiste = await _context.Productos
                .AnyAsync(p => p.Id == detalle.ProductoId);

            if (!pedidoExiste)
            {
                return BadRequest(new
                {
                    mensaje = "El pedido no existe"
                });
            }

            if (!productoExiste)
            {
                return BadRequest(new
                {
                    mensaje = "El producto no existe"
                });
            }

            _context.DetallesPedido.Add(detalle);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetDetalle),
                new { id = detalle.Id },
                detalle);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalle(int id)
        {
            var detalle = await _context.DetallesPedido
                .FindAsync(id);

            if (detalle == null)
                return NotFound();

            _context.DetallesPedido.Remove(detalle);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
```

------------------------------------------------------------------------

# 15. Ejecutar el proyecto

Desde Visual Studio:

``` text
▶ Ejecutar
```

La aplicación mostrará una URL similar a:

``` text
https://localhost:7000
```

El puerto puede ser diferente en cada instalación.

Swagger estará disponible en:

``` text
https://localhost:7000/swagger
```

------------------------------------------------------------------------

# 16. Probar con Postman

## Obtener clientes

``` http
GET https://localhost:7000/api/clientes
```

Respuesta aproximada:

``` json
[
  {
    "id": 1,
    "nombre": "Juan Pérez",
    "correo": "juan@gmail.com",
    "telefono": "3001111111",
    "pedidos": [
      {
        "id": 1,
        "fecha": "2026-09-01T09:30:00",
        "clienteId": 1,
        "cliente": null,
        "detalles": [
          {
            "id": 1,
            "cantidad": 1,
            "precio": 2500000,
            "pedidoId": 1,
            "pedido": null,
            "productoId": 1,
            "producto": {
              "id": 1,
              "nombre": "Laptop Lenovo",
              "precio": 2500000,
              "stock": 10,
              "categoriaId": 1,
              "categoria": null,
              "detalles": []
            }
          }
        ]
      }
    ]
  }
]
```

------------------------------------------------------------------------

# 17. Obtener un cliente

``` http
GET https://localhost:7000/api/clientes/1
```

------------------------------------------------------------------------

# 18. Crear cliente

Método:

``` http
POST https://localhost:7000/api/clientes
```

En Postman:

``` text
Body
  ↓
raw
  ↓
JSON
```

Enviar:

``` json
{
  "nombre": "Pedro Martínez",
  "correo": "pedro@gmail.com",
  "telefono": "3004444444"
}
```

------------------------------------------------------------------------

# 19. Actualizar cliente

``` http
PUT https://localhost:7000/api/clientes/1
```

Body:

``` json
{
  "id": 1,
  "nombre": "Juan Pérez Actualizado",
  "correo": "juan.nuevo@gmail.com",
  "telefono": "3009999999"
}
```

------------------------------------------------------------------------

# 20. Eliminar cliente

``` http
DELETE https://localhost:7000/api/clientes/3
```

Respuesta:

``` text
204 No Content
```

------------------------------------------------------------------------

# 21. Obtener pedidos

``` http
GET https://localhost:7000/api/pedidos
```

También se puede consultar:

``` http
GET https://localhost:7000/api/pedidos/1
```

La respuesta contiene:

``` text
Pedido
 ├── Cliente
 └── Detalles
      └── Producto
           └── Categoria
```

------------------------------------------------------------------------

# 22. Crear pedido

``` http
POST https://localhost:7000/api/pedidos
```

Body:

``` json
{
  "clienteId": 1
}
```

La API asignará automáticamente la fecha.

------------------------------------------------------------------------

# 23. Obtener productos

``` http
GET https://localhost:7000/api/productos
```

------------------------------------------------------------------------

# 24. Crear producto

``` http
POST https://localhost:7000/api/productos
```

Body:

``` json
{
  "nombre": "Webcam Logitech",
  "precio": 180000,
  "stock": 12,
  "categoriaId": 2
}
```

------------------------------------------------------------------------

# 25. Actualizar producto

``` http
PUT https://localhost:7000/api/productos/1
```

Body:

``` json
{
  "id": 1,
  "nombre": "Laptop Lenovo ThinkPad",
  "precio": 2800000,
  "stock": 7,
  "categoriaId": 1
}
```

------------------------------------------------------------------------

# 26. Eliminar producto

``` http
DELETE https://localhost:7000/api/productos/4
```

------------------------------------------------------------------------

# 27. Obtener categorías

``` http
GET https://localhost:7000/api/categorias
```

------------------------------------------------------------------------

# 28. Crear categoría

``` http
POST https://localhost:7000/api/categorias
```

Body:

``` json
{
  "nombre": "Impresoras"
}
```

------------------------------------------------------------------------

# 29. Obtener detalles de pedidos

``` http
GET https://localhost:7000/api/detallespedido
```

------------------------------------------------------------------------

# 30. Crear detalle de pedido

``` http
POST https://localhost:7000/api/detallespedido
```

Body:

``` json
{
  "cantidad": 2,
  "precio": 85000,
  "pedidoId": 1,
  "productoId": 2
}
```

------------------------------------------------------------------------

# 31. Resumen de endpoints

  Método   Endpoint                     Operación
  -------- ---------------------------- ----------------------
  GET      `/api/clientes`              Listar clientes
  GET      `/api/clientes/{id}`         Consultar cliente
  POST     `/api/clientes`              Crear cliente
  PUT      `/api/clientes/{id}`         Actualizar cliente
  DELETE   `/api/clientes/{id}`         Eliminar cliente
  GET      `/api/pedidos`               Listar pedidos
  GET      `/api/pedidos/{id}`          Consultar pedido
  POST     `/api/pedidos`               Crear pedido
  DELETE   `/api/pedidos/{id}`          Eliminar pedido
  GET      `/api/productos`             Listar productos
  GET      `/api/productos/{id}`        Consultar producto
  POST     `/api/productos`             Crear producto
  PUT      `/api/productos/{id}`        Actualizar producto
  DELETE   `/api/productos/{id}`        Eliminar producto
  GET      `/api/categorias`            Listar categorías
  GET      `/api/categorias/{id}`       Consultar categoría
  POST     `/api/categorias`            Crear categoría
  PUT      `/api/categorias/{id}`       Actualizar categoría
  DELETE   `/api/categorias/{id}`       Eliminar categoría
  GET      `/api/detallespedido`        Listar detalles
  GET      `/api/detallespedido/{id}`   Consultar detalle
  POST     `/api/detallespedido`        Crear detalle
  DELETE   `/api/detallespedido/{id}`   Eliminar detalle

------------------------------------------------------------------------

# 32. Flujo recomendado de pruebas en Postman

Para evitar errores de claves foráneas, probar en este orden:

``` text
1. Crear Categoría
        ↓
2. Crear Producto
        ↓
3. Crear Cliente
        ↓
4. Crear Pedido
        ↓
5. Crear DetallePedido
        ↓
6. Consultar Pedido
```

Ejemplo:

``` text
Categoria  → ID 1
     ↓
Producto   → ID 1
     ↓
Cliente    → ID 1
     ↓
Pedido     → ID 1
     ↓
Detalle    → Pedido 1 + Producto 1
```

------------------------------------------------------------------------

# 33. Comprobar MySQL

Después de ejecutar las peticiones POST, comprobar:

``` sql
USE tienda_db;

SELECT * FROM cliente;

SELECT * FROM categoria;

SELECT * FROM producto;

SELECT * FROM pedido;

SELECT * FROM detalle_pedido;
```

------------------------------------------------------------------------

# 34. Consulta SQL con JOIN

Para comprobar la información relacionada directamente desde MySQL:

``` sql
SELECT
    c.id AS cliente_id,
    c.nombre AS cliente,
    p.id AS pedido_id,
    p.fecha,
    pr.nombre AS producto,
    d.cantidad,
    d.precio,
    cat.nombre AS categoria
FROM cliente c
INNER JOIN pedido p
    ON c.id = p.cliente_id
INNER JOIN detalle_pedido d
    ON p.id = d.pedido_id
INNER JOIN producto pr
    ON d.producto_id = pr.id
INNER JOIN categoria cat
    ON pr.categoria_id = cat.id
ORDER BY c.id, p.id;
```

------------------------------------------------------------------------

# 35. Posibles errores

## Error: Unknown database

``` text
Unknown database 'tienda_db'
```

Solución:

``` sql
CREATE DATABASE tienda_db;
```

------------------------------------------------------------------------

## Error: Access denied

``` text
Access denied for user 'root'
```

Revisar:

``` json
"Server=localhost;Port=3306;Database=tienda_db;User=root;Password=;"
```

Colocar la contraseña correcta.

------------------------------------------------------------------------

## Error: Table doesn't exist

``` text
Table 'tienda_db.cliente' doesn't exist
```

Ejecutar nuevamente:

``` text
tienda_db.sql
```

------------------------------------------------------------------------

## Error de conexión

Comprobar que MySQL/MariaDB esté iniciado.

En XAMPP:

``` text
XAMPP
  ↓
MySQL
  ↓
Start
```

------------------------------------------------------------------------

# 36. Arquitectura

La aplicación utiliza una arquitectura sencilla:

``` text
Postman
   │
   ▼
ASP.NET Core Web API
   │
   ▼
Controllers
   │
   ▼
Entity Framework Core
   │
   ▼
ApplicationDbContext
   │
   ▼
Pomelo MySQL Provider
   │
   ▼
MySQL / MariaDB
```

------------------------------------------------------------------------

# 37. Tecnologías utilizadas

  Tecnología              Uso
  ----------------------- ----------------------
  C#                      Lenguaje
  .NET 10                 Framework
  ASP.NET Core            API REST
  Entity Framework Core   ORM
  Pomelo                  Proveedor MySQL
  MySQL/MariaDB           Base de datos
  Swagger                 Documentación
  Postman                 Pruebas
  JSON                    Intercambio de datos
  Visual Studio 2026      IDE

------------------------------------------------------------------------

# 38. Git

Inicializar repositorio:

``` bash
git init
```

Agregar archivos:

``` bash
git add .
```

Crear commit:

``` bash
git commit -m "API REST Tienda con C# y MySQL"
```

Conectar con GitHub:

``` bash
git remote add origin https://github.com/USUARIO/TiendaAPI.git
```

Subir:

``` bash
git branch -M master
git push -u origin master
```

------------------------------------------------------------------------

# 39. .gitignore recomendado

Crear `.gitignore`:

``` gitignore
## Visual Studio

.vs/
.vscode/

## Build

bin/
obj/

## User files

*.user
*.suo
*.userosscache
*.sln.docstates

## ASP.NET

appsettings.Development.json

## Logs

*.log

## NuGet

packages/

## Generated files

TestResults/

## Rider

.idea/

## OS

.DS_Store
Thumbs.db
```

> No subir contraseñas reales de MySQL al repositorio. Para un proyecto
> real, utilizar variables de entorno o secretos de desarrollo.

------------------------------------------------------------------------

# 40. Resultado final

El proyecto permite realizar procesamiento completo de información
mediante:

``` text
                 ┌─────────────┐
                 │   Postman   │
                 └──────┬──────┘
                        │
                        ▼
              ┌──────────────────┐
              │ ASP.NET Core API │
              └────────┬─────────┘
                       │
              ┌────────▼─────────┐
              │ Entity Framework │
              └────────┬─────────┘
                       │
                ┌──────▼───────┐
                │     MySQL     │
                └───────────────┘
```

Con cinco entidades:

``` text
Cliente
   │
   └── Pedido
          │
          └── DetallePedido
                    │
                    └── Producto
                           │
                           └── Categoria
```

Esto proporciona un proyecto completo para practicar **API REST + C# +
.NET 10 + Entity Framework Core + MySQL + relaciones + JSON + Postman**.

