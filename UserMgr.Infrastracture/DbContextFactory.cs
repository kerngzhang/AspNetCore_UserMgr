using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMgr.Infrastracture
{
    public class DbContextFactorypublic : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<UserDbContext>();
            builder.UseSqlServer("Server=kerng;Database=ddd1;Encrypt=True;TrustServerCertificate=True;Trusted_Connection=True;");
            return new UserDbContext(builder.Options);
        }
    }
}
