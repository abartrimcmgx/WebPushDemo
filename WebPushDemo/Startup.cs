using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebPushDemo.Data;

namespace WebPushDemo;

public class Startup(IConfiguration configuration)
{
    private IConfiguration Configuration { get; } = configuration;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc(options => options.EnableEndpointRouting = false);

        services.AddDbContext<WebPushDemoContext>(options =>
            options.UseSqlite("Data Source=Data/WebPushDb.db"));

        services.AddSingleton<IConfiguration>(Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
        app.UseBrowserLink();
        app.UseDeveloperExceptionPage();

        app.UseStaticFiles();

        if (Configuration.GetSection("VapidKeys")["PublicKey"]?.Length == 0 ||
            Configuration.GetSection("VapidKeys")["PrivateKey"]?.Length == 0)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=WebPush}/{action=GenerateKeys}/{id?}");
            });

            return;
        }

        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Devices}/{action=Index}/{id?}");
        });

        // Ensure the database is created.
        var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
        using var serviceScope = serviceScopeFactory.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<WebPushDemoContext>();
        context?.Database.EnsureCreated();
    }
}