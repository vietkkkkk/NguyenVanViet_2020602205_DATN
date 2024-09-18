using System.ComponentModel.DataAnnotations.Schema;
namespace webbanhang.Models
{
    [Table("Admin")]
    public class ItemAdmin
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
