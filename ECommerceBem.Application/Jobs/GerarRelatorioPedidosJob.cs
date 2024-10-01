using Quartz;
using ECommerceBem.Core.Enum;
using ECommerceBem.Core.Interfaces.Repositories;
using System.Text;
using ECommerceBem.Core.Entities;

namespace ECommerceBem.Application.Jobs;
public class GerarRelatorioPedidosJob : IJob
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly INotificacaoRepository _notificacaoRepository;

    public GerarRelatorioPedidosJob(IPedidoRepository pedidoRepository, INotificacaoRepository notificacaoRepository)
    {
        _pedidoRepository = pedidoRepository;
        _notificacaoRepository = notificacaoRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        // Obtém os pedidos do dia anterior
        var dataAnterior = DateTime.Now.AddDays(-1).Date;
        var pedidos = await _pedidoRepository.ObterPedidosPorDataAsync(dataAnterior);

        if (!pedidos.Any())
            return;

        var relatorio = gerarRelatorio(pedidos);

        var mensagemRelatorio = relatorio.ToString();
        var notificacao = new NotificacaoEntity(pedidoId: Guid.NewGuid(),
                                            descricao: mensagemRelatorio,
                                            tipoNotificacao: NotificacaoEnum.Email,
                                            data: DateTime.Now);

        await _notificacaoRepository.AdicionarAsync(notificacao);
    }

    private StringBuilder gerarRelatorio(IEnumerable<PedidoEntity> pedidos)
    {
        var relatorio = new StringBuilder();
        relatorio.AppendLine("Relatório de Pedidos do Dia Anterior:");
        relatorio.AppendLine("======================================");

        foreach (var pedido in pedidos)
        {
            relatorio.AppendLine($"Pedido ID: {pedido.Id}");
            relatorio.AppendLine($"Data: {pedido.DataPedido}");
            relatorio.AppendLine($"Valor Total: {pedido.ValorTotal:C}");
            relatorio.AppendLine($"Status: {pedido.Status}");
            relatorio.AppendLine("Itens:");

            foreach (var item in pedido.Itens)
            {
                relatorio.AppendLine($" - Produto: {item.Produto.Nome}, Quantidade: {item.Quantidade}, Total: {item.PrecoTotal:C}");
            }

            relatorio.AppendLine("--------------------------------------");
        }
        return relatorio;
    }
}