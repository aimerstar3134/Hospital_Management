using Hospital_Management.Areas.Patient.Data;
using Hospital_Management.Areas.User.Data;
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
builder.Services.AddScoped<user_layer>();  // Register as scoped

// Other service registrations
builder.Services.AddControllersWithViews();
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
                   pattern: "Doctor/{controller=Doctor}/{action=DoctorList}");

    endpoints.MapAreaControllerRoute(
                   name: "Patient",
                   areaName: "Patient",
                   pattern: "Patient/{controller=Patient}/{action=PatientDashboard}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

});

app.Run();

