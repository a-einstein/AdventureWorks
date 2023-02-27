var builder = WebApplication.CreateBuilder();

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<ProductsService>();
    serviceBuilder.AddServiceEndpoint<ProductsService, IProductsService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/ProductsService.svc");

    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();

    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.Run();
