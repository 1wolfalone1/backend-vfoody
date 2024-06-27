using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VFoody.Application.UseCases.Shop.Models
{
    public class SelectSimpleProductOfShopDTO
    {
        public int Id
        {
            get => ProductId;
        }

        public string Name
        {
            get => ProductName;
        }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int TotalOrder { get; set; }
        public int Status { get; set; }
        public int ShopId { get; set; }
        [JsonIgnore]
        public int TotalItems { get; set; }
        [JsonIgnore]
        public int TotalPages { get; set; }
        [JsonIgnore]
        public int ProductId { get; set; }
        [JsonIgnore]
        public string ProductName { get; set; }
    }
}
