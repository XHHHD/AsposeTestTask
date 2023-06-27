using Microsoft.EntityFrameworkCore;

namespace AsposeTestTask.DAL.Data
{
    public partial class AsposeContext : DbContext
    {
        public AsposeContext() => Database.EnsureCreated();

        public AsposeContext(DbContextOptions<AsposeContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
