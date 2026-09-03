using System.ComponentModel.DataAnnotations;

namespace TiendaAPI.Modelos
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