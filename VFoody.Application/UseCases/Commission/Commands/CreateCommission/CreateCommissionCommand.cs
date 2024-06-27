using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Commission.Commands.CreateCommission;

public class CreateCommissionCommand : ICommand<Result>
{
    public float CommissionRate { get; set; }
}