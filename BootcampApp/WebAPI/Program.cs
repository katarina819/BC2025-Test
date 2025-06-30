using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BootcampApp.Repository;
using BootcampApp.Repository.BootcampApp.Repository.DrinksRepository;
using BootcampApp.Service;
using BootcampApp.Service.BootcampApp.Service.DrinksService;
using Npgsql;
using BootcampApp.Service.BootcampApp.Service.PizzaService;


Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

// Obavezno postavi Autofac kao DI container:
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                          ?? throw new InvalidOperationException("Connection string not found.");

    containerBuilder.RegisterInstance(connectionString).As<string>().SingleInstance();

    containerBuilder.Register(c => new NpgsqlConnection(connectionString))
                    .As<NpgsqlConnection>()
                    .InstancePerLifetimeScope();

    containerBuilder.RegisterType<NotificationRepository>().As<INotificationRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<PaymentRepository>().AsSelf().InstancePerLifetimeScope();

    containerBuilder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<DrinkRepository>().As<IDrinkRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<PizzaRepository>().As<IPizzaRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<PizzaOrderRepository>().As<IPizzaOrderRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<DrinksOrderRepository>().As<IDrinksOrderRepository>().InstancePerLifetimeScope();

    containerBuilder.RegisterType<PaymentService>().AsSelf().InstancePerLifetimeScope();
    containerBuilder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<DrinkService>().As<IDrinkService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<PizzaService>().As<IPizzaService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<PizzaOrderService>().As<IPizzaOrderService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<DrinksOrderService>().As<IDrinksOrderService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<NotificationService>().As<INotificationService>().InstancePerLifetimeScope();
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5176")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // ➕ Ovo je obavezno za withCredentials
    });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();



var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
