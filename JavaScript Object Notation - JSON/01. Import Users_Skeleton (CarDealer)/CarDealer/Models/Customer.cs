using CarDealer.DTOs.Import;
using System.Runtime.CompilerServices;

namespace CarDealer.Models
{
    public class Customer
    {
        public Customer()
        {
            
        }
        public Customer(ImportCustomerDTO customerDTO)
            : base()
        {
            this.Name = customerDTO.Name;
            this.BirthDate = customerDTO.BirthDate;
            this.IsYoungDriver = customerDTO.IsYoungDriver;
        }
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public bool IsYoungDriver { get; set; }

        public ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();
    }
}