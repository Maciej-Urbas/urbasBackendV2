using Microsoft.EntityFrameworkCore;

using urbasBackendV2.Helpers;
using urbasBackendV2.Services;

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

// Implementacja serwisu wraz z interfejsem
// configure DI for application services
builder.Services.AddScoped<IMdUsersService, MdUsersService>();
builder.Services.AddTransient<IMdUsersService, MdUsersService>();

// Dodanie odniesienia do AutoMappera
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
