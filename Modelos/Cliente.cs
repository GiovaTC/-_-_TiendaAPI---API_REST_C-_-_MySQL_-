using System.ComponentModel.DataAnnotations;

namespace TiendaAPI.Modelos
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
