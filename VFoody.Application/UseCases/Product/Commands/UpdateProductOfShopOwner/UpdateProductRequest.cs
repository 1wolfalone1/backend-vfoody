using Microsoft.AspNetCore.Http;

namespace VFoody.Application.UseCases.Product.Commands.UpdateProductOfShopOwner;

public class UpdateProductRequest
{
    public int Id { get; set; }
    public string ImgUrl { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public List<int> CategoryIds { get; set; }
    public List<UpdateQuestionRequest>? questions { get; set; }

    public class UpdateQuestionRequest
    {
        public int? Id { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public List<UpdateOptionRequest> options { get; set; }
    }

    public class UpdateOptionRequest
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public ulong IsPricing { get; set; }
        public string ImgUrl { get; set; }
        public float Price { get; set; }
        public int Status { get; set; }
    }
}