namespace SolutionsForBusiness
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var startup = new Startup(builder.Environment);
            startup.ConfigureServices(builder.Services);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            startup.Configure(app, builder.Environment);
            app.Run();
        }
    }
}