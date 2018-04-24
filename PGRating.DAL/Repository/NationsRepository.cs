using PGRating.DAL.DataContext;
using PGRating.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace PGRating.DAL.Repository
{
    public class NationsRepository
    {
        public async Task<List<Nation>> GetNationsAsync()
        {
            using (var db = new CivlDataContext())
            {
                return await db.Nations.ToListAsync();
            }
        }

        public async Task SaveNationsAsync(IList<Nation> nations)
        {
            using (var db = new CivlDataContext())
            {
                db.Nations.AddRange(nations);

                await db.SaveChangesAsync();
            }
        }

        public async Task ClearNationsAsync()
        {
            using (var db = new CivlDataContext())
            {
                var nations = await db.Nations.ToListAsync();
                db.Nations.RemoveRange(nations);

                await db.SaveChangesAsync();
            }
        }
    }
}
