using ECommerceBem.Application.Events;
using ECommerceBem.Application.Jobs;
using ECommerceBem.Application.Services;
using ECommerceBem.Application.Services.Interfaces;
using ECommerceBem.Core.Interfaces.Repositories;
using ECommerceBem.Filters;
using ECommerceBem.Infrastructure.DBContext;
using ECommerceBem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Quartz
builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("GerarRelatorioPedidosJob");
    q.AddJob<GerarRelatorioPedidosJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("GerarRelatorioPedidosJob-trigger")
        .WithCronSchedule("0 0 0 * * ?")); // Executa à meia-noite todos os dias
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(NotificacaoEventHandler).Assembly));

// Configurar o DbContext com SQLite
builder.Services.AddDbContext<ECommerceBemDBContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMvc(config => config.Filters.Add(typeof(ExceptionFilter)));

// Configurações de Serviços
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<INotificacaoRepository, NotificacaoRepository>();

builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IPagamentoService, PagamentoService>();
builder.Services.AddScoped<IEntregaService, EntregaService>();
builder.Services.AddScoped<IEstoqueService, EstoqueService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Chama o seeder para popular os produtos
DataSeeder.Seed(app.Services);
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ECommerceBemDBContext>();

    var produtos = context.Produtos.ToList();

    Console.WriteLine("Produtos disponíveis no banco de dados:");
    foreach (var produto in produtos)
    {
        Console.WriteLine($"ID: {produto.Id}, Nome: {produto.Nome}, Preco: {produto.PrecoUnitario}, QNTD: {produto.QuantidadeEmEstoque}");
    }
}

// Configurar para escutar em todas as interfaces e na porta 80
builder.WebHost.UseUrls("http://0.0.0.0:80");

// Configurações do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
