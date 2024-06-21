using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VFoody.Application.UseCases.Product.Models;

namespace VFoody.Application.UseCases.Shop.Models
{
    public class SelectDetailsShopDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsFavouriteShop { get; set; }
        public string? LogoUrl { get; set; }
        public string? BannerUrl { get; set; }
        public string? Description { get; set; }
        public int ActiveFrom { get; set; }
        public double Rating { get; set; }
        public string PhoneNumber { get; set; }
        public int BuildingId { get; set; }
        public string BuildingName { get; set; }
        public int ActiveTo { get; set; }
        public bool Active { get; set; }
        public int TotalOrder { get; set; }
        public int TotalProduct { get; set; }
        public int TotalRating { get; set; }
        public int TotalStar { get; set; }
        public int Status { get; set; }
        public float MinimumValueOrderFreeship { get; set; }
        public float ShippingFee { get; set; }
        public int AccountId { get; set; }

        public List<SelectSimpleProductOfShopDTO> Products { get; set; }

        [JsonIgnore]
        public int TotalItems { get; set; }
        [JsonIgnore]
        public int TotalPages { get; set; }
    }
}
