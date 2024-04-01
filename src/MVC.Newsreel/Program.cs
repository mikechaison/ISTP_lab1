using MVC.Newsreel.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MVC.Newsreel.Services;
using MVC.Newsreel.Data.Identity;
using System.Reflection;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Lab1dbContext>(options => 
    {options.UseSqlServer(builder.Configuration.GetConnectionString("Lab1dbContext"));
    options.ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored));
    });
builder.Services.AddDbContext<ApplicationIdentityContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(ApplicationIdentityContext)),
sqlOptions => sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name)));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => 
options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationIdentityContext>();
builder.Services.AddTransient<ArticleDataPortServiceFactory>();
builder.Services.AddTransient<ArticleImportService>();
builder.Services.AddTransient<ArticleExportService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
