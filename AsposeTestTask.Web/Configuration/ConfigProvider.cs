namespace AsposeTestTask.Web.Configuration
{
    public class ConfigProvider : IConfigProvider
    {
        string IConfigProvider.GetDbConnectionString()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            return config.GetConnectionString("DefaultConnection");
        }
    }
}
