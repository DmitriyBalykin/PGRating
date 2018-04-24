using PGRating.DAL.DataContext;
using PGRating.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGRating.DAL.Repository
{
    public class CompetitionsRepository
    {
        public List<Competition> GetCompetitions()
        {
            using (var db = new CivlDataContext())
            {
                return db.Competitions.ToList();
            }
        }

        public async Task SaveCompetitions(List<Competition> competitions)
        {
            using (var db = new CivlDataContext())
            {
                db.Competitions.AddRange(competitions);

                await db.SaveChangesAsync();
            }
        }
    }
}
