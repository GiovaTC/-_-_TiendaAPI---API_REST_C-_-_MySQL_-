using System.ComponentModel.DataAnnotations;

namespace TiendaAPI.Modelos
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