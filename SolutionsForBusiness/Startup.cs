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
            services.AddTransient<IProviderRepository, ProviderRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IItemRepository, ItemRepository>();

            //services.AddScoped<IService<Infrastructure.DataBase.Models.Provider, Provider>, ProviderService>();
            //services.AddScoped<IService<Order>, OrderService>();
            //services.AddScoped<IService<Infrastructure.DataBase.Models.Item, Item>, ItemService>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IProviderService, ProviderService>();

            //services.AddHttpContextAccessor();
            services.AddMvc();
        }

        public void Configure(WebApplication app, IHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            //app.UseEndpoints(builder => )
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{page?}");
            //app.UseMvcWithDefaultRoute();
        }
    }
}