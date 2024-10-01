using ECommerceBem.Core.Enum;
using ECommerceBem.Core.Events;
using ECommerceBem.Core.Resource;
using ECommerceBem.Exception;
using ECommerceBem.Exception.ExceptionsBase;

namespace ECommerceBem.Core.Entities;

public class PedidoEntity
{
    private List<IDomainEvent> _domainEvents;
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

    public Guid Id { get; private set; }
    public List<ItemPedidoEntity> Itens { get; private set; } = [];
    public decimal ValorTotal { get; private set; }
    public StatusPedido Status { get; private set; }
    public DateTime DataPedido { get; private set; } 
    public FormaPagamentoEnum FormaPagamento { get; private set; }

    public PedidoEntity(FormaPagamentoEnum formaPagamento)
    {
        Id = Guid.NewGuid();
        Status = StatusPedido.AguardandoProcessamento;
        DataPedido = DateTime.Now;
        FormaPagamento = formaPagamento;

        adicionarEventoNotificacao(ResourceNotificacao.PedidoCriado, NotificacaoEnum.PedidoCriado);
    }

    private void adicionarEventoNotificacao(string mensagemTemplate, NotificacaoEnum tipoNotificacao)
    {
        var mensagem = string.Format(mensagemTemplate, Id, DataPedido);
        AdicionarEventoDominio(new NotificacaoEvent(Id, mensagem, NotificacaoEnum.PedidoAguardandoEstoque));
    }
    private void atualizarValorTotal(ItemPedidoEntity item)
    {
        ValorTotal += item.PrecoTotal;
    }
    private void alterarStatus(StatusPedido novoStatus, NotificacaoEnum notificacaoTipo, string mensagemTemplate)
    {
        Status = novoStatus;
        adicionarEventoNotificacao(mensagemTemplate, notificacaoTipo);
    }

    public void AdicionarItem(ItemPedidoEntity item)
    {
        Itens.Add(item);
        atualizarValorTotal(item);
    }

    public void DefinirStatusAguardandoEstoque()
    {
        if (Status != StatusPedido.SeparandoPedido)
            throw new NotFoundException(string.Format(ResourceErrorsMessages.PedidoStatusErro, Status));

        Status = StatusPedido.AguardandoEstoque;
        var mensagem = string.Format(ResourceNotificacao.PedidoAguardandoEstoque, Id);
        alterarStatus(StatusPedido.AguardandoEstoque, NotificacaoEnum.PedidoAguardandoEstoque, mensagem);
        
        var mensagemVendas = string.Format(ResourceNotificacao.PedidoSemEtoque, Id);;
        AdicionarEventoDominio(new NotificacaoEvent(Id, mensagemVendas, NotificacaoEnum.Vendas));
    }

    public void DefinirStatusSeparandoPedido()
    {
        if (Status != StatusPedido.PagamentoConcluido)
            throw new NotFoundException(string.Format(ResourceErrorsMessages.PedidoStatusErro, Status));

        var mensagem = string.Format(ResourceNotificacao.PedidoSeparado, Id);
        alterarStatus(StatusPedido.SeparandoPedido, NotificacaoEnum.PedidoEmSeparacao, mensagem);
    }

    public void DefinirStatusProcessandoPagamento()
    {
        if (Status != StatusPedido.AguardandoProcessamento)
            throw new NotFoundException(string.Format(ResourceErrorsMessages.PedidoStatusErro, Status));

        var mensagem = string.Format(ResourceNotificacao.PedidoProcessado, Id);
        alterarStatus(StatusPedido.ProcessandoPagamento, NotificacaoEnum.PagamentoProcessando, mensagem);
    }

    public void AplicarDescontoPix(decimal desconto)
    {
        var descontoAplicado = ValorTotal * desconto;
        ValorTotal -= descontoAplicado;
    }

    public void DefinirStatusPagamentoConcluido()
    {
        if (Status != StatusPedido.ProcessandoPagamento)
            throw new NotFoundException(string.Format(ResourceErrorsMessages.PedidoStatusErro, Status));

        var mensagem = string.Format(ResourceNotificacao.PedidoPago, Id);
        alterarStatus(StatusPedido.PagamentoConcluido, NotificacaoEnum.PagamentoConcluido, mensagem);
    }

    public void DefinirStatusConcluido()
    {
        if (Status != StatusPedido.SeparandoPedido)
            throw new NotFoundException(string.Format(ResourceErrorsMessages.PedidoStatusErro, Status));

        var mensagem = string.Format(ResourceNotificacao.PedidoConcluido, Id);
        alterarStatus(StatusPedido.Concluido, NotificacaoEnum.PedidoConcluido, mensagem);
    }

    public void Cancelar()
    {
        switch (Status)
        {
            case StatusPedido.AguardandoProcessamento:
            case StatusPedido.AguardandoEstoque:
                // Pode ser cancelado diretamente
                alterarStatus(
                    StatusPedido.Cancelado, 
                    NotificacaoEnum.PedidoCancelado, 
                    string.Format(ResourceNotificacao.PedidoCancelado,Id)
                );
                break;

            case StatusPedido.ProcessandoPagamento:
            case StatusPedido.PagamentoConcluido:
                // Necessita de estorno
                alterarStatus(
                    StatusPedido.Cancelado, 
                    NotificacaoEnum.PedidoCancelado, 
                    string.Format(ResourceNotificacao.PedidoEstornado, Id));
                break;

            case StatusPedido.Concluido:
            case StatusPedido.SeparandoPedido:
                throw new NotFoundException(ResourceErrorsMessages.ErroCancelamento);
            default:
                throw new NotFoundException(ResourceErrorsMessages.PedidoStatusErro);
        }
    }

    public void AdicionarEventoDominio(IDomainEvent domainEvent)
    {
        _domainEvents ??= new List<IDomainEvent>();
        _domainEvents.Add(domainEvent);
    }
    public void LimparEventosDominio() => _domainEvents?.Clear();
}
