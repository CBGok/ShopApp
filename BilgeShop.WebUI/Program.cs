using BilgeShop.Business.Managers;
using BilgeShop.Business.Services;
using BilgeShop.Data.Context;
using BilgeShop.Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


var connectionString = builder.Configuration.GetConnectionString("HomeConnection");
builder.Services.AddDbContext<BilgeShopContext>(options => options.UseSqlServer(connectionString));




builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IProductService, ProductManager>();



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/");
    options.LogoutPath = new PathString("/");
    options.AccessDeniedPath = new PathString("/");
});

var contentRootPath = builder.Environment.ContentRootPath;

var keysDirectory = new DirectoryInfo(Path.Combine(contentRootPath, "App-Data", "Keys"));

builder.Services.AddDataProtection()
    .SetApplicationName("BilgeShop")
    .SetDefaultKeyLifetime(new TimeSpan(999999, 0, 0, 0))
    .PersistKeysToFileSystem(keysDirectory);

// App_Data -> Keys -> Ýçerisindeki xml dosyasýna sahip her proje ayný þifreleme/þifre açma yöntemi kullanacaðýndan, birbirlerinin þifrelerini açabilirler.

var app = builder.Build();


app.UseStaticFiles(); // wwwroot için


app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithRedirects("/Errors/Error{0}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "Default",
    pattern: "{Controller=Home}/{Action=Index}/{id?}"
    );



app.Run();





