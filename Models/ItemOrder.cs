using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace webbanhang.Models
{
    [Table("Orders")]
    public class ItemOrder
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime Create { get; set; }
        public double Price { get; set; }
        public int Status { get; set; }
    }
}
