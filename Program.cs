
//using Hospital_Management.Areas.Patient.Data;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.Configuration;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<PatientDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("connection")));
//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options=>
//options.IdleTimeout = TimeSpan.FromDays(15));
//builder.Services.AddMvc();
//builder.Services.AddControllersWithViews();
//builder.Services.AddHttpContextAccessor();
//// Add services to the container.
//builder.Services.AddControllersWithViews();
//var app = builder.Build();
//app.UseSession();
//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();


//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapAreaControllerRoute(
//                       name: "Doctor",
//                       areaName: "Doctor",
//                       pattern: "Doctor/{controller=Doctor}/{action=DoctorProfileList}");

//    endpoints.MapAreaControllerRoute(
//                       name: "Patient",
//                       areaName: "Patient",
//                       pattern: "Patient/{controller=Patient}/{action=AppoinmnetList}");

//    endpoints.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=User}/{action=Login}/{id?}");

//});

//app.Run();

using Hospital_Management.Areas.Patient.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext and configure session
builder.Services.AddDbContext<PatientDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection")));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
    options.IdleTimeout = TimeSpan.FromDays(15));

// Add MVC and HttpContextAccessor
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Adjust as needed
        options.LoginPath = "/User/Login"; // Your login page
    });

var app = builder.Build();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
                   name: "Doctor",
                   areaName: "Doctor",
                   pattern: "Doctor/{controller=Doctor}/{action=DoctorProfileList}");

    endpoints.MapAreaControllerRoute(
                   name: "Patient",
                   areaName: "Patient",
                   pattern: "Patient/{controller=Patient}/{action=AppoinmnetList}");

    endpoints.MapAreaControllerRoute(
                   name: "Staff",
                   areaName: "Staff",
                   pattern: "Staff/{controller=Staff}/{action=StaffDashboard}");

    endpoints.MapAreaControllerRoute(
                   name: "Admin",
                   areaName: "Admin",
                   pattern: "Admin/{controller=Admin}/{action=AdminDashboard}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=User}/{action=Login}/{id?}");
});

app.Run();

