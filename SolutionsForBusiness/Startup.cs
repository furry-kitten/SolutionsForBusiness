using Infrastructure.DataBase;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;
using PresentationLayer.Services;

namespace SolutionsForBusiness
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostEnvironment environment) =>
            _configuration = new ConfigurationBuilder().SetBasePath(environment.ContentRootPath)
                                                       .AddJsonFile("dbSettings.json")
                                                       .Build();

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _configuration.GetConnectionString("CreatingString");
            services.AddDbContext<EfCoreContext>(builder => builder.UseSqlServer(connectionString, optionsBuilder => optionsBuilder.MigrationsAssembly(nameof(Infrastructure))));
            services.AddTransient<ProviderRepository>();
            services.AddTransient<OrderRepository>();
            services.AddTransient<ItemRepository>();

            services.AddScoped<IService<Provider>, ProviderService>();
            services.AddScoped<IService<Order>, OrderService>();
            services.AddScoped<IService<OrderItem>, ItemService>();

            //services.AddHttpContextAccessor();

            services.AddMvc();
        }

        public void Configure(WebApplication app, IHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            //app.UseEndpoints(builder => )
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            //app.UseMvcWithDefaultRoute();
        }
    }
}