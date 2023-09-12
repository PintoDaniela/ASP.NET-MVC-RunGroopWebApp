using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>> GetAll();
        Task<Club> GetByIdAsync(int id);
        Task<Club> GetByIdAsyncNoTracking(int id);
        Task<IEnumerable<Club>> GetClubsByCity(string city);
        bool AddClub(Club club);
        bool DeleteClub(Club club);
        bool Save();
        bool UpdateClub(Club club);

    }
}
