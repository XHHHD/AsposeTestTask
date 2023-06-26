using AsposeTestTask.BLL.Interfaces;
using AsposeTestTask.BLL.Services;
using AsposeTestTask.DAL.Data;
using AsposeTestTask.Services;
using AsposeTestTask.Web.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AsposeTestTask.Web.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddDataBaseContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddEntityFrameworkProxies();

            builder.Services.AddDbContext<AsposeContext>((serviceProvider, options) =>
            {
                var configProvider = serviceProvider.GetService<IConfigProvider>();
                var connectionString = configProvider.GetDbConnectionString();
                options.UseInternalServiceProvider(serviceProvider)
                    .UseLazyLoadingProxies()
                    .UseSqlServer(connectionString);
            });
        }
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ICompanyService, CompanyService>();
            builder.Services.AddTransient<IPersonService, PersonService>();
        }
    }
}
