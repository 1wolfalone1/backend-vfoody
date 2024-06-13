using Microsoft.AspNetCore.Http;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Commands.UpdateProductOfShopOwner;

public class UpdateProductCommand : ICommand<Result>
{
    public int Id { get; set; }
    public string ImgUrl { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public List<int> CategoryIds { get; set; }
    public List<UpdateQuestionCommand>? Questions { get; set; }

    public class UpdateQuestionCommand
    {
        public int? Id { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public List<UpdateOptionCommand> Options { get; set; }
    }

    public class UpdateOptionCommand
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public ulong IsPricing { get; set; }
        public string ImgUrl { get; set; }
        public float Price { get; set; }
        public int Status { get; set; }
    }
}