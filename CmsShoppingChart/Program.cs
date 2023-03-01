using CmsShoppingChart.Infrastructure;
using CmsShoppingChart.Services;
using Microsoft.EntityFrameworkCore;

public class program
{
    public static void Main(String[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //var services = builder.Services;
        builder.Services.AddDbContext<CmsShoppingContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ShoppingConnection")));
        // Add services to the container.
        builder.Services.AddControllersWithViews();
       // builder.Services.Initialize();


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
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );
            endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        //app.MapControllerRoute(
        //    name: "default",
        //    pattern: "{controller=Home}/{action=Index}/{id?}");
        
        SeedData.Initialize(app);

        app.Run();
      

    }
}