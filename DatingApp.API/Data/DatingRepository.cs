using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            this._context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            //throw new System.NotImplementedException();
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            //throw new System.NotImplementedException();
            _context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            //throw new System.NotImplementedException();
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            //throw new System.NotImplementedException();
            var users = await _context.Users.Include(p => p.Photos).ToListAsync();
            return users;

        }

        public async Task<bool> SaveAll()
        {
            //throw new System.NotImplementedException();
            return await _context.SaveChangesAsync() > 0;
        }
    }
}