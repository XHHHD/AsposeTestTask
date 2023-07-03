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
            ///builder.Services.AddEntityFrameworkProxies();    <-- This is why I spended three day more then expected. In EF7 this service was simplified for LazyLoading.

            builder.Services.AddDbContextPool<AsposeContext>((serviceProvider, options) =>
            {
                var configProvider = serviceProvider.GetService<IConfigProvider>();
                var connectionString = configProvider.GetDbConnectionString();

                options
                    //.UseInternalServiceProvider(serviceProvider)  <-- This also took few days of my time. In a result, I was forced to retreat.
                    .UseSqlServer(connectionString)
                    .UseLazyLoadingProxies();
            });
        }
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ICompanyService, CompanyService>();
            builder.Services.AddTransient<IPersonService, PersonService>();
            ///I didn't add the CancellationToken coz I was running out of time. But all services accept it, so CancellationToken can be added at any time.
        }
    }
}
