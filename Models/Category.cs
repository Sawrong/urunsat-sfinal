using System.ComponentModel.DataAnnotations;

namespace UrunSatisPortali.Models
{
    public class Category
    {
        [Key]
        public int kategoriID { get; set; }
        public string kategoriAD { get; set; }
        public string kategoriAciklama { get; set; }
        public IList<Product> products { get; set; }

    }
}
