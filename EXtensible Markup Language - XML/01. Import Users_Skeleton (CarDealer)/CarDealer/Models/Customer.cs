using CarDealer.DTOs.Import;

namespace CarDealer.Models
{
    public class Customer
    {
        public Customer()
        {
            this.Sales = new HashSet<Sale>();
        }
        public Customer(ImportCustomersDTO customersDTO)
            : this()
        {
            this.Name= customersDTO.Name;
            this.BirthDate = customersDTO.BirthDate;
            this.IsYoungDriver = customersDTO.IsYoungDriver;
        }
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public bool IsYoungDriver { get; set; }

        public ICollection<Sale> Sales { get; set; }
    }
}