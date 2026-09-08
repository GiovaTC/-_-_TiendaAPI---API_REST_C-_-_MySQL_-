using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaAPI.Data;
using TiendaAPI.Modelos;

namespace TiendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context) {
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
