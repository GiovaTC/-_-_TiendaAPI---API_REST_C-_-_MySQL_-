using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaAPI.Data;
using TiendaAPI.Modelos;


namespace TiendaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetallesPedidoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DetallesPedidoController(ApplicationDbContext context)
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
            {
                return NotFound();
            }

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
            
            if (!productoExiste)
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
        public async Task<ActionResult> DeleteDetalle(int id)
        {
            var detalle = await _context.DetallesPedido
                .FindAsync(id);

            if (detalle == null)
            {
                return NotFound();
            }

            _context.DetallesPedido.Remove(detalle);
            
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}   
