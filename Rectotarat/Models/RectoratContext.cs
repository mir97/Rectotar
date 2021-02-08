using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rectotarat.Models;

namespace Rectotarat.Models
{



    public partial class RectoratContext : IdentityDbContext<User>
    {
        public RectoratContext()
        {
        }

        public RectoratContext(DbContextOptions<RectoratContext> options) : base(options)
        {
        }

        public virtual DbSet<Indicator> Indicators { get; set; }
        public virtual DbSet<University> Universitys { get; set; }
        public virtual DbSet<Achievement> Achievements { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<Chairperson> Chairpersons { get; set; }
        public virtual DbSet<Rector> Rectors { get; set; }
        public virtual DbSet<Raschet> Raschets { get; set; }
        public virtual DbSet<News> Newss { get; set; }
        public DbSet<Rectotarat.Models.User> User { get; set; }

    }
}
