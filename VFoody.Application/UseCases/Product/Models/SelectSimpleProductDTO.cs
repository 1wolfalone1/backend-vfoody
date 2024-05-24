using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VFoody.Application.UseCases.Product.Models
{
    public class SelectSimpleProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int TotalOrder { get; set; }
        public bool Status { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopLogoUrl { get; set; }
        public bool ShopActive { get; set; }
        public int ShopActiveFrom { get; set; }
        public int ShopActiveTo { get; set; }
        [JsonIgnore]
        public int TotalItems { get; set; }
        [JsonIgnore]
        public int TotalPages { get; set; }
    }
}
