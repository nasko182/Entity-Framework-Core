using CarDealer.DTOs.Import;

namespace CarDealer.Models
{
    public class Sale
    {
        public Sale()
        {

        }
        public Sale(ImportSaleDTO saleDTO)
            : this()
        {
            this.CarId = saleDTO.CarId;
            this.CustomerId = saleDTO.CustomerId;
            this.Discount= saleDTO.Discount;
        }
        public int Id { get; set; }

        public decimal Discount { get; set; }

        public int CarId { get; set; }
        public Car Car { get; set; } = null!;    

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!; 
    }
}
