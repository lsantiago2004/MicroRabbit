using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Banking.Data.Context
{
    class BankingDBContextFactory : IDesignTimeDbContextFactory<BankingDBContext>
    {
        public BankingDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BankingDBContext>();
            // optionsBuilder.UseSqlServer("Server=LSANTIAGO-PC\\MSSQLSERVER_LUIS,Database=BankingDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            optionsBuilder.UseSqlServer("Data Source=LSANTIAGO-PC\\MSSQLSERVER_LUIS;Initial Catalog = BankingDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            //Data Source = (localdb)\\ProjectsV13; Initial Catalog = BankingDB; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False
            return new BankingDBContext(optionsBuilder.Options);
        }
    }
}
