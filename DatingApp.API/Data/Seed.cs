using System.Collections.Generic;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        private readonly DataContext _context;
        public Seed(DataContext context)
        {
            this._context = context;
        }

        public void SeedUser()
        {
            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            foreach (var user in users)
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash("password", out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PaswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();

                _context.Users.Add(user);
            }
            _context.SaveChanges();
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //Dispose after finishing since HMAC implement IDispose
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                //randomly generated key 
                passwordSalt = hmac.Key;
                //hashPassword with the key generated
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}