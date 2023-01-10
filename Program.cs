using Microsoft.EntityFrameworkCore;
using urbasBackendV2.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Rejestrowanie kontekstu BD wewnątz aplikacji (InMemoryDatabase)
// Pozniej zamienic na polaczenie z BD
// builder.Services.AddDbContext<UbContext>(opt => opt.UseInMemoryDatabase("MasterDesigner"));

// Nawiazanie polaczenia z BD
builder.Services.AddDbContext<UbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("urbasBackendV2Connection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
