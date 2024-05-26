namespace VFoody.Application.UseCases.Product.Models;

public class ProductDetailResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public List<QuestionResponse> Questions { get; set; }

    public class QuestionResponse
    {
        public int Id { get; set; }
        public int QuestionType { get; set; }
        public string Description { get; set; }
        public List<OptionResponse> Options { get; set; }
    }

    public class OptionResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsPricing { get; set; }
        public float Price { get; set; }
        public int Status { get; set; }
        public string ImageUrl { get; set; }
    }
}