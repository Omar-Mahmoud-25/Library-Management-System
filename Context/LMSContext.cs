using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Context
{
    public class LMSContext : DbContext
    {
        public LMSContext(DbContextOptions<LMSContext> options)
            : base(options)
        {
        }
    }
}