using PGRating.DAL.DataContext;
using PGRating.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGRating.DAL.Repository
{
    public class PilotsRepository
    {
        public List<Pilot> GetPilots()
        {
            using (var db = new CivlDataContext())
            {
                return db.Pilots.ToList();
            }
        }

        public async Task SavePilots(List<Pilot> pilots)
        {
            using (var db = new CivlDataContext())
            {
                db.Pilots.AddRange(pilots);

                await db.SaveChangesAsync();
            }
        }
    }
}
