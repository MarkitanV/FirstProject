
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FirstProject.Domain;
using FirstProject.Domain.Repositories.Abstract;
using FirstProject.Domain.Repositories.EntityFramework;
using FirstProject.Service;




var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;





//���������� �������� ��

//services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Config.ConnectionString));


// �������� ������ ����������� �� ����� ������������
//string connection = builder.Configuration.GetConnectionString("Project");

services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("Server=DESKTOP-6MQDCBJ\\SQLEXPRESS;Database=ProjectDataBase; Persist Security Info=false; User ID='sa'; Password='sa'; MultipleActiveResultSets=True; Trusted_Connection=False;"));


//���������� ������ ���������� ���������� � �������� ��������
services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>();
services.AddTransient<IServiceItemsRepository, EFServiceItemsRepository>();
services.AddTransient<DataManager>();


//��������� ������� ��� ������������ � ������������� (MVC)
services.AddControllersWithViews();








//����������� identity �������
services.AddIdentity<IdentityUser, IdentityRole>(opts =>
{
    opts.User.RequireUniqueEmail = true;
    opts.Password.RequiredLength = 6;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

//����������� authentication cookie
services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "myCompanyAuth";
    options.Cookie.HttpOnly = true;
    options.LoginPath = "/account/login";
    options.AccessDeniedPath = "/account/accessdenied";
    options.SlidingExpiration = true;
});

//����������� �������� ����������� ��� Admin area
services.AddAuthorization(x =>
{
    x.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
});

// ��������� ������� ��� ������������ � ������������� MVC

services.AddControllersWithViews(x =>
{
    x.Conventions.Add(new AdminAreaAuthorization("Admin", "AdminArea"));
});

var app = builder.Build();

IConfiguration configuration = app.Configuration;

app.Configuration.Bind("Project", new Config());


app.UseStaticFiles();

//���������� �������������� � �����������r
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(name: "admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");



app.Run();
