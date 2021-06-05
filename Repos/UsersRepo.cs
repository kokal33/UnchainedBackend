using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnchainedBackend.Data;
using UnchainedBackend.Models;
using UnchainedBackend.Models.ReturnModels;

namespace UnchainedBackend.Repos
{
    public interface IUsersRepo 
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(string publicAddress);
        Task<IEnumerable<ArtistsReturn>> GetArtists();
        Task<bool> PostUser(User user);
        Task<bool> DeleteUser(string publicAddress);
        Task<bool> UpdateUser(User user);
        Task<bool> UnchainUser(string publicAddress);
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

        public async Task<User> GetUser(string publicAddress)
        {
            return await _context.Users.FindAsync(publicAddress);
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
            // Persist the signature because we dont get it on update
            user.Signature = existingUser.Signature;
            user.Verified = existingUser.Verified;
            if (existingUser.IsArtist == false && user.IsArtist == true)
            {
                PendingArtist pendingArtist = new() { ArtistPublicAddress = user.PublicAddress };
                await _context.PendingArtists.AddAsync(pendingArtist);
            }

            _context.Entry(existingUser).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ArtistsReturn>> GetArtists()
        {
            return await _context.Users.Where(x => x.IsArtist).Select(a => new ArtistsReturn()
            {
                Name = a.Name,
                ProfilePic = a.ProfilePic,
                Verified = a.Verified,
                PublicAddress = a.PublicAddress
            }).ToListAsync();
        }

        public async Task<bool> UnchainUser(string publicAddress)
        {
            var user = new User { PublicAddress = publicAddress, Verified = true };
            _context.Entry(user).Property(x => x.Verified).IsModified = true;

            var pendingArtist = await _context.PendingArtists.Where(x => x.ArtistPublicAddress == publicAddress)
                .FirstOrDefaultAsync();
            _context.PendingArtists.Remove(pendingArtist);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
