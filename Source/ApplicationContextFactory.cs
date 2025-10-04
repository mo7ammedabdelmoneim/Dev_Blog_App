using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source
{
    internal class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var optionBuilder = new DbContextOptionsBuilder<ApplicationContext>();

            optionBuilder.UseSqlServer("Data Source=MOHAMED-ABDELMO\\SQLEXPRESS;Initial Catalog=MiniBlogAAppDB;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");

            return new ApplicationContext(optionBuilder.Options);
        }
    }
}
