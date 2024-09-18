using System.ComponentModel.DataAnnotations.Schema;
namespace webbanhang.Models
{
    [Table("Categories")]
    public class ItemCategory
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public int DisplayHomePage { get; set; }
    }
}
