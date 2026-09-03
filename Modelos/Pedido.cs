using System.ComponentModel.DataAnnotations;

namespace TiendaAPI.Modelos
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