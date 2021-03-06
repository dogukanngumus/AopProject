using Core.Entities;
using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework.Contexts;

public class NorthwindContext:DbContext
{
   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
      optionsBuilder.UseSqlServer("Server=localhost;Database=Northwind;User Id=sa;Password=Nodsid.2091;");
   }

   public DbSet<Product> Products { get; set; }
   public DbSet<User> Users { get; set; }
   public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
   public DbSet<OperationClaim> OperationClaims { get; set; }
}