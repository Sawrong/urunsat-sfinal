using System.ComponentModel.DataAnnotations;

namespace UrunSatisPortali.Models
{
    public class Product
    {
        [Key]
        public int productID { get; set; }
        public string productName { get; set; }
        public int productStock { get; set; }
        public string productExplain { get; set; }
        public string productImage { get; set; }
        public decimal productPrice { get; set; }

        public int categoryID { get; set; }
        public Category categoryies { get; set; }
    }
}
