using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// ?? Registracija kontrolera (obavezno!)
builder.Services.AddControllers();

// ?? Swagger (nije obavezno, ali korisno)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Ovdje dohvaæaš connection string iz appsettings.json ---
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                          ?? throw new InvalidOperationException("Connection string not found.");
builder.Services.AddScoped<UserService>(sp => new UserService(connectionString));

// Registriraj UserService u DI (kao scoped ili singleton)
builder.Services.AddScoped<UserService>(sp => new UserService(connectionString));

var app = builder.Build();

// ?? Swagger UI za razvoj
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ?? HTTPS redirekcija i autorizacija
app.UseHttpsRedirection();
app.UseAuthorization();

// ?? Mapiranje kontrolera na URL rute (obavezno!)
app.MapControllers();

app.Run();

