using Microsoft.EntityFrameworkCore;
using StudentManager.Data;
using StudentManager.Services;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<StudentContext>(options =>
            options.UseSqlite(
                builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IStudentService, StudentService>();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Students}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}