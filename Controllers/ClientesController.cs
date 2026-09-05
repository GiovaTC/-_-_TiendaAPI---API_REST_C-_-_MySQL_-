using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaAPI.Data;
using TiendaAPI.Modelos;

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
