using System.Threading.Tasks;
using DevixonApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevixonApi.Data.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Image> Images { get; set; }
        Task<int> SaveChangesAsync();
    }
}