using InventoryManagement.Api.DTOs;
using InventoryManagement.Domain;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Domain.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IRepository<StockItem>, InMemoryRepository<StockItem>>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", ()=> "Hello brrroooo");

app.MapGet("/stock-items", (string? status, IRepository<StockItem> repository) => 
{
    var items = repository.GetAll();

    if (!string.IsNullOrWhiteSpace(status))
    {
        if(!Enum.TryParse<StockStatus>(status, true, out var parseStatus) || !Enum.IsDefined(parseStatus))
        {
            return Results.BadRequest("Invalid stock status");
        }

        items = items.Where(item => item.Status == parseStatus).ToList();
    }

    return Results.Ok(items);
});

app.MapGet("/stock-items/{id:guid}", (Guid id, IRepository<StockItem> repository) =>
{
    var item = repository.GetById(id);

    if (item == null)
            return Results.NotFound("Item not found");

    return Results.Ok(item);
});

app.MapPost("/stock-items", (CreateStockItemRequest request, IRepository<StockItem> repository) =>
{
    if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.SKU))
    {
        return Results.BadRequest("Name and SKu are required");
    }

    if (request.Quantity < 0 || request.Price < 0)
    {
        return Results.BadRequest("Quantity and price can't be negative");
    }

    Product product;

    if (string.IsNullOrWhiteSpace(request.Description))
    {
        product = new Product() { Name = request.Name, SKU = request.SKU };
    }
    else
    {
        product = new Product(request.Name, request.SKU, request.Description) { Name = request.Name, SKU = request.SKU};
    }

    var stockItem = new StockItem(product, request.Quantity, request.Price);

    repository.Add(stockItem);

    return Results.Created($"/stock-items/{stockItem.Id}", stockItem);
});

app.MapPost("/stock-items/{id:guid}/issue", (Guid id, IRepository<StockItem> repository, IssueStockItemRequest request) =>
{
    if (request.Quantity <= 0)
    {
        return Results.BadRequest("Quantity can't be negative");
    }

    var item = repository.GetById(id);

    if (item == null) 
    { 
        return Results.NotFound("Item not found");
    }
    try
    {
        item.DecreaseQuantity(request.Quantity);
    }
    catch (InvalidOperationException ex) 
    {
        return Results.Conflict(ex.Message);
    }

    return Results.Ok(item);
});

app.MapPost("/stock-items/{id:guid}/receive", (Guid id, ReceiveStockItemRequest request, IRepository<StockItem> repository) =>
{
    if (request.Quantity <= 0)
        return Results.BadRequest("Quantity can't be negative");

    var item = repository.GetById(id);

    if (item == null)
        return Results.NotFound("Item not found");

    item.IncreaseQuantity(request.Quantity);

    return Results.Ok(item);
});

app.MapPatch("/stock-items/{id:guid}", (Guid id, UpdateStockItemRequest request, IRepository<StockItem> repository) =>
{
    var item = repository.GetById(id);

    if (item == null)
        return Results.NotFound("Item not found");

    if (request.Quantity < 0 || request.Price < 0)
        return Results.BadRequest("Invalid quantity or price");

    try
    {
        if (request.Quantity.HasValue)
        {
            int difference = request.Quantity.Value - item.Quantity;

            if (difference > 0)
                item.IncreaseQuantity(difference);
            else if (difference < 0)
                item.DecreaseQuantity(-difference);
        }

        if (request.Price.HasValue)
            item.UpdatePrice(request.Price.Value);

        if (request.Description != null)
            item.Product.UpdateDescription(request.Description);

        return Results.Ok(item);
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(ex.Message);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/stock-items/{id:guid}", (Guid id, IRepository<StockItem> repository) =>
{
    var item = repository.GetById(id);

    if (item == null)
        return Results.NotFound("Item not found");

    repository.Remove(item);

    return Results.NoContent();
});

app.Run();
