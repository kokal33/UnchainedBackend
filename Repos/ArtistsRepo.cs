using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnchainedBackend.Data;
using UnchainedBackend.Models;

namespace UnchainedBackend.Repos
{
    public interface ITracksRepo
    {
        Task<IEnumerable<Track>> GetTracks();
        Task<Track> GetTrack(int id);
        Task<bool> PostTrack(Track track);
        Task<bool> DeleteTrack(int id);
    }
    public class TracksRepo : ITracksRepo
    {
        private readonly ApplicationContext _context;

        public TracksRepo(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Track>> GetTracks()
        {
            return await _context.Tracks.ToListAsync();
        }

        public async Task<Track> GetTrack(int id)
        {
            var track = await _context.Tracks.Include(x => x.OwnerOf).FirstOrDefaultAsync(t => t.Id == id);
            return track;
        }       

        public async Task<bool> PostTrack(Track track)
        {
            _context.Tracks.Add(track);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTrack(int id)
        {
            var track = await _context.Tracks.FindAsync(id);

            _context.Tracks.Remove(track);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
