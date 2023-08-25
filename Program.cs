using Domain;
using Repository;
using Repository.Interface;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRepository<Shape>, ShapeRepository>();   
builder.Services.AddScoped<IRepository<ShapeData>, ShapeDataRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

}

//app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Shape}/{action=Index}/{id?}");

Environment.SetEnvironmentVariable("GEOSERVER_HOME", @"C:\GeoServer");
var geoserver = new Process
{
    StartInfo = new ProcessStartInfo
    {
        FileName = "cmd",
        Arguments = "/C start \"GeoServer\" \"C:\\geoserver\\bin\\startup.bat",
    }
};
geoserver.Start();

app.Run();
