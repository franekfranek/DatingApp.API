using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;

        public DatingRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
            //this is not asyns because we just save it to context we dont do anything with the db
            //it will be save in memory UNTIL WE ACTUALLY SAVE IT TO THE DB
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
            //no async same explaination as for Add method
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(p => p.Photos)
                                     .FirstOrDefaultAsync(u => u.Id == id);
            //photos here are considered naviation property
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.Include(p => p.Photos).ToListAsync();

            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
            //if more than 1 change is done it returns true or nothing- false 
        }
    }
}
