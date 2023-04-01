using CarDealer.DTOs.Import;

namespace CarDealer.Models
{
    public class Supplier
    {
        public Supplier()
        {
            this.Parts = new HashSet<Part>();
        }
        public Supplier(ImportSupplierDTO supplierDTO)
            : this()
        {
            this.Name = supplierDTO.Name;
            this.IsImporter = supplierDTO.IsImporter;
        }
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public bool IsImporter { get; set; }

        public ICollection<Part> Parts { get; set; }
    }
}
