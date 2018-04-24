using PGRating.DAL.DataContext;
using PGRating.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGRating.DAL.Repository
{
    public class NationalParticipantRepository
    {
        public List<NationTeamParticipant> GetNationalParticipants()
        {
            using (var db = new CivlDataContext())
            {
                return db.NationParticipants.ToList();
            }
        }

        public async Task SaveNationalParticipants(List<NationTeamParticipant> participants)
        {
            using (var db = new CivlDataContext())
            {
                db.NationParticipants.AddRange(participants);

                await db.SaveChangesAsync();
            }
        }
    }
}
