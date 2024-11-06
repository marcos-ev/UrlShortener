using Microsoft.EntityFrameworkCore;
using urlshorter.Data;
using urlshorter.Services;
using urlshorter.Handlers; // Namespace correto para BasicAuthenticationHandler
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Configura��es de autentica��o
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddAuthorization();

// Adiciona suporte para controladores com views (MVC)
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UrlService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Configura��o de rota padr�o para o MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AccessLog}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
