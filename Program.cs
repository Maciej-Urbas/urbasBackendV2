using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

// Dodanie serwisu MdUsersTokenService wraz z interfejsem
builder.Services.AddScoped<IMdUsersTokenService, MdUsersTokenService>();

// Wdrozenie Autoryzacji i Autentykacji dla mdUsers
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration ["TokenKey"])),
                        ValidateIssuer = false,

                        ValidateAudience = false,
                    };
                });

// Implementacja serwisu MdUsersService wraz z interfejsem
builder.Services.AddScoped<IMdUsersService, MdUsersService>();
// Implementacja serwisu MdMessagesService wraz z interfejsem
builder.Services.AddScoped<IMdMessagesService, MdMessagesService>();

// AddCors pozwoli na wysylanie zapytan z innych stron np. przez fetch()
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Pozwolenie na Cross-Origin Resource Sharing
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();    

// Wdrozenie Autoryzacji i Autentykacji dla mdUsers ciag dalszy
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
