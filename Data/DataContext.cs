using Microsoft.EntityFrameworkCore;
using System;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options): base(options){}
	
		public DbSet<Value> Values { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Photo> Photos { get; set; }
		public DbSet<Like> Likes { get; set; }
		public DbSet<Message> Message { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Like>()
				.HasKey(k => new { k.LikerId, k.LikeeId });

			builder.Entity<Like>()
				.HasOne(u => u.Likee)
				.WithMany(u => u.Likers)
				.HasForeignKey(u => u.LikeeId)
				.OnDelete(DeleteBehavior.Restrict);//no cascade deletions(here no user deletion)
			
			builder.Entity<Like>()
				.HasOne(u => u.Liker)
				.WithMany(u => u.Likees)
				.HasForeignKey(u => u.LikerId)
				.OnDelete(DeleteBehavior.Restrict);//no cascade deletions

			builder.Entity<Message>()
				.HasOne(x => x.Sender)
				.WithMany(m => m.MessagesSent)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<Message>()
				.HasOne(x => x.Recipient)
				.WithMany(m => m.MessagesReceived)
				.OnDelete(DeleteBehavior.Restrict);
		}

		
	}
}
