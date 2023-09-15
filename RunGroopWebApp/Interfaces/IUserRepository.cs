using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser> GetById(string id);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Add (AppUser user);
        bool Save();
    }
}
