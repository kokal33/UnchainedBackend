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
        Task<Artist> GetArtist(int id);
        Task<bool> PostArtist(Artist artist);
        Task<bool> DeleteArtist(int id);
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

        public async Task<Artist> GetArtist(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            return artist;
        }       

        public async Task<bool> PostArtist(Artist artist)
        {
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteArtist(int id)
        {
            var artist = await _context.Artists.FindAsync(id);

            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
