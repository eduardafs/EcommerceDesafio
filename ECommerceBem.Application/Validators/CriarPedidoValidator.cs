using ECommerceBem.Application.Dto.Request;
using ECommerceBem.Exception;
using FluentValidation;

namespace ECommerceBem.Application.Validators;

public class CriarPedidoValidator : AbstractValidator<RequestCriarPedidoDto>
{
    public CriarPedidoValidator()
    {
        RuleFor(pedido => pedido.Itens)
            .NotEmpty().WithMessage(ResourceErrorsMessages.PedidoItensObrigatorio)
            .Must(itens => itens.All(item => item.Quantidade > 0))
            .WithMessage(ResourceErrorsMessages.ItemQuantidadeInvalida);

        RuleFor(pedido => pedido.FormaPagamento)
               .NotNull().WithMessage(ResourceErrorsMessages.FormaPagamentoObrigatoria)
               .IsInEnum().WithMessage(ResourceErrorsMessages.FormaPagamentoInvalida);

        RuleForEach(pedido => pedido.Itens).SetValidator(new ItemPedidoValidator());
    }
}

public class ItemPedidoValidator : AbstractValidator<ItemPedidoRequestDto>
{
    public ItemPedidoValidator()
    {
        RuleFor(item => item.Quantidade)
           .GreaterThan(0).WithMessage(ResourceErrorsMessages.ItemQuantidadeInvalida);
    }
}