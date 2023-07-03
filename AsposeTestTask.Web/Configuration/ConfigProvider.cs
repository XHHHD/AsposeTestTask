namespace AsposeTestTask.Web.Configuration
{
    public class ConfigProvider : IConfigProvider
    {
        public string GetDbConnectionString()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");    ///Connection string can bee changed after publishing program.
            var config = builder.Build();
            return config.GetConnectionString("DefaultConnection");
        }
    }
}
