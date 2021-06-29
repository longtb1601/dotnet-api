using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public class SampleWebApiContext : DbContext
    {
        public SampleWebApiContext(DbContextOptions<SampleWebApiContext> options) : base(options)
        {
        }

        public DbSet<TaskModel> TaskItems { get; set; }
    }
}