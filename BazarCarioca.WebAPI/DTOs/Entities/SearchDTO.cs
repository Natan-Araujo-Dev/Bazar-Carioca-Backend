namespace BazarCarioca.WebAPI.DTOs.Entities
{
    public class SearchDTO
    {
        public IEnumerable<StoreDTO> Stores { get; set; } = new List<StoreDTO>();
        public IEnumerable<ServiceDTO> Services { get; set; } = new List<ServiceDTO>();
        public IEnumerable<ProductTypeDTO> ProductTypes { get; set; } = new List<ProductTypeDTO>();
        public IEnumerable<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
