using MagicVilla_Web;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingConfig)); //add service of automapper 

builder.Services.AddHttpClient<IVillaService, VillaService>();//add http client
builder.Services.AddScoped<IVillaService, VillaService>(); // Add Villa Service

builder.Services.AddHttpClient<IVillaNumberService, VillaNumberService>();//add http client
builder.Services.AddScoped<IVillaNumberService, VillaNumberService>(); // Add VillaNumber Service

builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService,AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
