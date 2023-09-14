using RunGroopWebApp.Models;

namespace RunGroopWebApp.Interfaces
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetAll();
        Task<Race> GetByIdAsync(int id);
        Task<Race> GetByIdAsyncNoTracking(int id);
        Task<IEnumerable<Race>> GetRacesByCity(string city);
        bool AddRace(Race race);
        bool DeleteRace(Race race);
        bool Save();
        bool UpdateRace(Race race);
    }
}
