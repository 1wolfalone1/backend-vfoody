using Microsoft.AspNetCore.Http;

namespace VFoody.Application.UseCases.Product.Commands.CreateProductOfShopOwner;

public class CreateProductRequest
{
    public string ImgUrl { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public List<int> CategoryIds { get; set; }
    public List<CreateQuestionRequest>? Questions { get; set; }

    public class CreateQuestionRequest
    {
        public int Type { get; set; }
        public string Description { get; set; }
        public List<CreateOptionRequest> Options { get; set; }
    }

    public class CreateOptionRequest
    {
        public string Description { get; set; }
        public ulong IsPricing { get; set; }
        public string ImgUrl { get; set; }
        public float Price { get; set; }
    }
}