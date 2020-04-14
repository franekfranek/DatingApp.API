using Microsoft.EntityFrameworkCore;
using System;

/// <summary>
/// </summary>
public class DataContext : DbContext
{

	public DataContext(DbContextOptions<DataContext> options): base(options){}
	
	 public DbSet<Value> Values { get; set; }
}
