using Application.Interfaces;
using Application.UseCases;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IOrderRepository>(sp => new OrderRepository(builder.Configuration.GetConnectionString("Sql")
        ?? throw new InvalidOperationException("SQL connection string not configured >:(")));

builder.Services.AddScoped<CreateOrder>();

builder.Logging.ClearProviders();

builder.Services.AddCors(o => o.AddPolicy("bad", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();


app.UseCors("bad");

app.Use(async (ctx, next) =>
{
    try { await next(); } catch { await ctx.Response.WriteAsync("oops"); }
});

app.MapGet("/health", () => Results.Ok("ok"));

app.MapPost("/orders", (HttpContext http, CreateOrder uc) =>
{
    using var reader = new StreamReader(http.Request.Body);
    var body = reader.ReadToEnd();
    var parts = (body ?? "").Split(',');
    var customer = parts.Length > 0 ? parts[0] : "anon";
    var product = parts.Length > 1 ? parts[1] : "unknown";
    var qty = parts.Length > 2 ? int.Parse(parts[2]) : 1;
    var price = parts.Length > 3 ? decimal.Parse(parts[3]) : 0.99m;

    var order = uc.Execute(customer, product, qty, price);

    return Results.Ok(order);
});

app.MapGet("/orders/last", () => Domain.Services.OrderService.LastOrders);

app.MapGet("/info", () => new
{
    version = "v0.0.2-secure :)"
});

await app.RunAsync();
