using DatingApp.API.Helpers;
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

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(x => x.UserId == userId).FirstOrDefaultAsync(x => x.IsMain);
        }

        public async Task<Photo> GetPhoto(int id)
        {

            return await _context.Photos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(p => p.Photos)
                                     .FirstOrDefaultAsync(u => u.Id == id);
            //photos here are considered naviation property
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            //recently added first
            var users = _context.Users
                .Include(p => p.Photos).OrderByDescending(x => x.LastActive).AsQueryable();

            //filter out current user
            users = users.Where(x => x.Id != userParams.UserId);
            // filiter out same gender, it works because of the way userParams are set in UsersController 
            users = users.Where(x => x.Gender == userParams.Gender);


            //age filtering
            if(userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

                users = users.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);
            }
            //soerting
            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(x => x.Created);
                        break;
                    default:
                        users = users.OrderByDescending(x => x.LastActive);
                        break;
                }
            }
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
            //if more than 1 change is done it returns true or nothing- false 
        }
    }
}
