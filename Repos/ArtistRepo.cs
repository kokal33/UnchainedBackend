using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnchainedBackend.Data;
using UnchainedBackend.Models;

namespace UnchainedBackend.Repos
{
    public interface IArtistsRepo
    {
        Task<IEnumerable<Artist>> GetArtists();
        Task<Artist> GetArtist(string publicAddress);
        Task<bool> PostArtist(Artist artist);
        Task<bool> DeleteArtist(string publicAddress);
    }
    public class ArtistsRepo : IArtistsRepo
    {
        private readonly ApplicationContext _context;

        public ArtistsRepo(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Artist>> GetArtists()
        {
            return await _context.Artists.ToListAsync();
        }

        public async Task<Artist> GetArtist(string publicAddress)
        {
            var artist = await _context.Artists.FindAsync(publicAddress);
            return artist;
        }

        public async Task<bool> PostArtist(Artist artist)
        {
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteArtist(string publicAddress)
        {
            var artist = await _context.Artists.FindAsync(publicAddress);

            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserToArtist(Artist model)
        {
            var user = _context.Users.Find(model.PublicAddress);
            if (user == null)
                return false;

            _context.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
