using FileImageService.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileImageService.DataAccessLayer.Concrete
{
	public class Context : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("data source=MILANOPC;initial catalog=RannaTaskTwoDb;user id=ps;password=12345Aa!;MultipleActiveResultSets=True;App=EntityFramework;TrustServerCertificate=True");
		}
		public DbSet<Product> Products { get; set; }
		
    }
}
