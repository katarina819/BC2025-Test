var builder = WebApplication.CreateBuilder(args);

// ?? Registracija kontrolera (obavezno!)
builder.Services.AddControllers();

// ?? Swagger (nije obavezno, ali korisno)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

