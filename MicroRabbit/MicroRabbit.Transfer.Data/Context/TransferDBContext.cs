using MicroRabbit.Transfer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Transfer.Data.Context
{
    public class TransferDBContext : DbContext
    {
        public TransferDBContext(DbContextOptions<TransferDBContext> options ): base(options)
        {
        }
        public TransferDBContext() : base()
        {
        }


        public DbSet<TransferLog> TransferLogs { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["GhostContainer"].ConnectionString);
        //    optionsBuilder.UseSqlServer("Server=LSANTIAGO-PC\\MSSQLSERVER_LUIS, Database=BankingDB;Trusted_Connection=True;MultipleActiveResultSets=true");
        //}
    }
}
