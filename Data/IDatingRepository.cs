using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity)
            where T : class;
        void Delete<T>(T entity)
            where T : class;
        Task<bool> SaveAll();
        //it returns a boolean: if there is any change to our db then we return true
        // or false which means no changes to save or there were some problems with saving
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForUser(int userId);
    }
}
