using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer.DTOs.Import;

public class ImportSupplierDTO
{
    public ImportSupplierDTO()
    {

    }
    public ImportSupplierDTO(Supplier supplier)
        : base()
    {
        this.Name = supplier.Name;
        this.IsImporter = supplier.IsImporter;
    }

    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("isIpmorter")]
    public bool IsImporter { get; set; }
}
