using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceBem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NOTIFICACAO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PedidoId = table.Column<Guid>(type: "VARCHAR(100)", nullable: false),
                    Descricao = table.Column<string>(type: "VARCHAR(500)", nullable: false),
                    TipoNotificacao = table.Column<int>(type: "INT", nullable: false),
                    Data = table.Column<DateTime>(type: "DATETIME2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NOTIFICACAO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PEDIDO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    Status = table.Column<int>(type: "VARCHAR(100)", nullable: false),
                    DataPedido = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    FormaPagamento = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PEDIDO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PRODUTO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(40)", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "INT", nullable: false),
                    QuantidadeEmEstoque = table.Column<int>(type: "INT", nullable: false),
                    QuantidadeMinimaParaDesconto = table.Column<int>(type: "INT", nullable: false),
                    DescontoPorQuantidade = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    MesComDescontoSazonal = table.Column<int>(type: "INT", nullable: false),
                    DescontoSazonalPercentual = table.Column<decimal>(type: "decimal(5, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ITENS_PEDIDO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PedidoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Quantidade = table.Column<int>(type: "INT", nullable: false),
                    PrecoTotal = table.Column<decimal>(type: "decimal(5, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITENS_PEDIDO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ITENS_PEDIDO_PEDIDO_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "PEDIDO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ITENS_PEDIDO_PRODUTO_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "PRODUTO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ITENS_PEDIDO_PedidoId",
                table: "ITENS_PEDIDO",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_ITENS_PEDIDO_ProdutoId",
                table: "ITENS_PEDIDO",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ITENS_PEDIDO");

            migrationBuilder.DropTable(
                name: "NOTIFICACAO");

            migrationBuilder.DropTable(
                name: "PEDIDO");

            migrationBuilder.DropTable(
                name: "PRODUTO");
        }
    }
}
