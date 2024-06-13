using Microsoft.AspNetCore.Http;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Commands.CreateProductOfShopOwner;

public class CreateProductCommand : ICommand<Result>
{
    public string ImgUrl { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public List<int> CategoryIds { get; set; }
    public List<CreateQuestionCommand>? Questions { get; set; }

    public class CreateQuestionCommand
    {
        public int Type { get; set; }
        public string Description { get; set; }
        public List<CreateOptionCommand> Options { get; set; }
    }

    public class CreateOptionCommand
    {
        public string Description { get; set; }
        public ulong IsPricing { get; set; }
        public string ImgUrl { get; set; }
        public float Price { get; set; }
    }
}