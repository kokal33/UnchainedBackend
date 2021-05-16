using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnchainedBackend.Data;
using UnchainedBackend.Helpers;
using UnchainedBackend.Models;
using UnchainedBackend.Models.PartialModels;

namespace UnchainedBackend.Repos
{
    public interface IUsersRepo 
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(SignatureModel model);
        Task<bool> PostUser(User user);
        Task<bool> DeleteUser(string publicAddress);
        Task<bool> UpdateUser(User user);

    }
    public class UsersRepo : IUsersRepo
    {
        private readonly ApplicationContext _context;

        public UsersRepo(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUser(SignatureModel model)
        {
            var user = await _context.Users.FindAsync(model.PublicAddress);
            return user;
        }       

        public async Task<bool> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUser(string publicAddress)
        {
            var user = await _context.Users.FindAsync(publicAddress);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUser(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x=>x.PublicAddress == user.PublicAddress);
            if (existingUser.IsArtist == false && user.IsArtist == true)
            {
                PendingArtist pendingArtist = new() { ArtistPublicAddress = user.PublicAddress };
                await _context.PendingArtists.AddAsync(pendingArtist);
            }

            _context.Entry(existingUser).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetArtists()
        {
            return await _context.Users.Where(x=>x.IsArtist==true).ToListAsync();
        }
    }
}
