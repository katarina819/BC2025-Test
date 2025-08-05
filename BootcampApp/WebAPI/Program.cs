using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BootcampApp.SignalR.Hubs;
using BootcampApp.Repository;
using BootcampApp.Repository.BootcampApp.Repository.DrinksRepository;
using BootcampApp.Service;
using BootcampApp.Service.BootcampApp.Service.DrinksService;
using BootcampApp.Service.BootcampApp.Service.PizzaService;
using Npgsql;

// Register code page provider for text encoding support (e.g. for certain DB encodings)
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

// Configure Autofac as the Dependency Injection container instead of default Microsoft DI
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());



// Configure the Autofac container with registrations
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Get connection string from configuration
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                          ?? throw new InvalidOperationException("Connection string not found.");

    // Register the connection string as a singleton string instance
    containerBuilder.RegisterInstance(connectionString).As<string>().SingleInstance();

    // Register NpgsqlConnection with scoped lifetime per request
    containerBuilder.Register(c => new NpgsqlConnection(connectionString))
                    .As<NpgsqlConnection>()
                    .InstancePerLifetimeScope();

    // Register repositories with scoped lifetime
    containerBuilder.RegisterType<NotificationRepository>().As<INotificationRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<PaymentRepository>().AsSelf().InstancePerLifetimeScope();

    containerBuilder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<DrinkRepository>().As<IDrinkRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<PizzaRepository>().As<IPizzaRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<PizzaOrderRepository>().As<IPizzaOrderRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<DrinksOrderRepository>().As<IDrinksOrderRepository>().InstancePerLifetimeScope();

    // Register services with scoped lifetime
    containerBuilder.RegisterType<PaymentService>().AsSelf().InstancePerLifetimeScope();
    containerBuilder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<DrinkService>().As<IDrinkService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<PizzaService>().As<IPizzaService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<PizzaOrderService>().As<IPizzaOrderService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<DrinksOrderService>().As<IDrinksOrderService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<NotificationService>().As<INotificationService>().InstancePerLifetimeScope();
});

// Add controllers and configure JSON options to:
// - Ignore circular references to prevent serialization issues
// - Ignore null values in the JSON output
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Configure CORS policy to allow frontend app hosted at localhost:5175 to access this API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://pizzadrinks.netlify.app")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Required for allowing cookies or credentials to be sent
    });
});

// Add API documentation generator (Swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "BootcampApp API",
        Version = "v1"  
    });
});

builder.Services.AddSignalR();
builder.Services.AddSingleton<Microsoft.AspNetCore.SignalR.IUserIdProvider, NameIdentifierUserIdProvider>();

builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.Cookie.Name = "BootcampAppAuth";
        options.LoginPath = "/api/users/login"; 
        options.AccessDeniedPath = "/access-denied";
    });

builder.Services.AddAuthorization();


// Configure logging to only use console logger, clearing other providers
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Use CORS policy defined earlier
app.UseCors("AllowFrontend");

// Development-only middleware: Developer exception page + Swagger UI for API testing
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enforce HTTPS redirection
app.UseHttpsRedirection();

app.UseAuthentication();

// Enable authorization middleware (for [Authorize] attributes if used)
app.UseAuthorization();

app.MapHub<NotificationHub>("/notificationHub").RequireCors("AllowFrontend");



// Map controller routes for incoming requests
app.MapControllers();

// Run the application
app.Run();
