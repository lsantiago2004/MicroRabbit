using MicroRabbit.Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Banking.Data.Context
{
    public class BankingDBContext : DbContext
    {
        public BankingDBContext(DbContextOptions<BankingDBContext> options ): base(options)
        {
        }
        public BankingDBContext() : base()
        {
        }


        public DbSet<Account> Accounts { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["GhostContainer"].ConnectionString);
        //    optionsBuilder.UseSqlServer("Server=LSANTIAGO-PC\\MSSQLSERVER_LUIS, Database=BankingDB;Trusted_Connection=True;MultipleActiveResultSets=true");
        //}
    }
}
