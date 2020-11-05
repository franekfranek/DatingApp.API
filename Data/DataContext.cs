using Microsoft.EntityFrameworkCore;
using System;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Data
{
	/// <summary>
	/// all this classes are specified here becase we use int in Id property
	/// for migration
	/// </summary>
	public class DataContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole,
								IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
	{
		public DataContext(DbContextOptions<DataContext> options): base(options){}
	
		public DbSet<Value> Values { get; set; }
		//public DbSet<User> Users { get; set; } 
		// it can be removed due to the fact IdentityDbContext already has a Users table
		public DbSet<Photo> Photos { get; set; }
		public DbSet<Like> Likes { get; set; }
		public DbSet<Message> Message { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder); // because of Identity


			builder.Entity<UserRole>(userRole =>
			{
				userRole.HasKey(ur => new
				{
					ur.UserId,
					ur.RoleId
				});

				userRole.HasOne(ur => ur.Role)
				.WithMany(r => r.UserRoles)
				.HasForeignKey(ur => ur.RoleId)
				.IsRequired();

				userRole.HasOne(ur => ur.User)
				.WithMany(r => r.UserRoles)
				.HasForeignKey(ur => ur.UserId)
				.IsRequired();
			});


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

			builder.Entity<Photo>().HasQueryFilter(p => p.IsApproved);
		}

		
	}
}
