namespace UrunSatisPortali.Models
{
    public class UrunEkle
    {
        public int productID { get; set; }
        public string productName { get; set; }
        public int productStock { get; set; }
        public string productExplain { get; set; }
        public IFormFile productImage { get; set; }
        public decimal productPrice { get; set; }

        public int categoryID { get; set; }
    }
}
