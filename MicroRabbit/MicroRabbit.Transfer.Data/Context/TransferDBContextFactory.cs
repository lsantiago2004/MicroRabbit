using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using MicroRabbit.Transfer.Data.Context;

namespace MicroRabbit.Banking.Data.Context
{
    class TransferDBContextFactory : IDesignTimeDbContextFactory<TransferDBContext>
    {
        public TransferDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TransferDBContext>();
            // optionsBuilder.UseSqlServer("Server=LSANTIAGO-PC\\MSSQLSERVER_LUIS,Database=BankingDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            optionsBuilder.UseSqlServer("Data Source=LSANTIAGO-PC\\MSSQLSERVER_LUIS;Initial Catalog = TransferDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            //Data Source = (localdb)\\ProjectsV13; Initial Catalog = BankingDB; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False
            return new TransferDBContext(optionsBuilder.Options);
        }
    }
}
