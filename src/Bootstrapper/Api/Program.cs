var builder = WebApplication.CreateBuilder(args);
var catalogAssembly = typeof(CatalogModule).Assembly;
builder.Services
    .AddCarterWithAssemblies(catalogAssembly);
builder.Services
    .AddMediatRWithAssemblies(catalogAssembly);

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

var app = builder.Build();
app.MapCarter();
app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.Run();
