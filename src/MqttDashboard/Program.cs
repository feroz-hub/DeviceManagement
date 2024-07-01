using Mapster;
using Microsoft.EntityFrameworkCore;
using MqttDashboard.Data;
using MqttDashboard.Infrastructure;
using MqttDashboard.Models;
using MqttDashboard.Services;
using MqttHub.Extension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IMessageReceiverService, MessageReceiverService>();
builder.Services.AddScoped<IMqttClientRepository, MqttClientRepository>();
builder.Services.AddMqttService(typeof(Program).Assembly);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
