using System;
using System.Collections.Generic;
using System.Linq; 

namespace DatingApp.API.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public byte[] PasswordHash { get; set; }
		public byte[] PasswordSalt { get; set; }



		public User()
		{
			

		}
	}
}
