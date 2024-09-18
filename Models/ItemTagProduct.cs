using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace webbanhang.Models
{
    [Table("TagsProducts")]
    public class ItemTagProduct
    {
        [Key]
        public int Id { get; set; }
        public int TagId { get; set; }
        public int ProductId { get; set; }
    }
}
