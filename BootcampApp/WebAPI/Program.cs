using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BootcampApp.Repository;
using BootcampApp.Service;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

// === Umjesto defaultnog ServiceProvider-a koristi Autofac ===
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// === Autofac konfiguracija za DI ===
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                          ?? throw new InvalidOperationException("Connection string not found.");

    // Registriraj repozitorije
    containerBuilder.Register(c =>
    {
        var logger = c.Resolve<ILogger<UserRepository>>();
        return new UserRepository(connectionString, logger);
    }).As<IUserRepository>().InstancePerLifetimeScope();

    containerBuilder.Register(c =>
    {
        var logger = c.Resolve<ILogger<PizzaOrderRepository>>();
        return new PizzaOrderRepository(connectionString, logger);
    }).As<IPizzaOrderRepository>().InstancePerLifetimeScope();

    containerBuilder.Register(c =>
    {
        var logger = c.Resolve<ILogger<PizzaRepository>>();
        return new PizzaRepository(connectionString, logger);
    }).As<IPizzaRepository>().InstancePerLifetimeScope();

    containerBuilder.Register(c =>
    {
        var logger = c.Resolve<ILogger<DrinksOrderRepository>>();
        return new DrinksOrderRepository(connectionString, logger);
    }).As<IDrinksOrderRepository>().InstancePerLifetimeScope();

    containerBuilder.RegisterType<DrinksOrderService>().As<IDrinksOrderService>().InstancePerLifetimeScope();


    // Registriraj servise
    containerBuilder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();

    containerBuilder.RegisterType<PizzaOrderService>()
    .As<IPizzaOrderService>()
    .InstancePerLifetimeScope();

});

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
        opts.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Logger setup
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();



