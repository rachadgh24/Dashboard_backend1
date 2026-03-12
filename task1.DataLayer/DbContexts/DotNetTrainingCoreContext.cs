using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task1.DataLayer.Entities;

namespace task1.DataLayer.DbContexts
{
    public class DotNetTrainingCoreContext : DbContext
    {
      public DotNetTrainingCoreContext(DbContextOptions<DotNetTrainingCoreContext> options)
            : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
