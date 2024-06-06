using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Firebases.Commands.VerifyIdTokens;

public class VerifyIDTokensCommand : ICommand<Result>
{
    public string IdToken { get; set; }
}