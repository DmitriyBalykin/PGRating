using PGRating.DAL.DataContext;
using PGRating.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace PGRating.DAL.Repository
{
    public class NationalParticipantRepository
    {
        public Task<List<NationTeamParticipant>> GetNationalParticipantsAsync()
        {
            using (var db = new CivlDataContext())
            {
                return db.NationParticipants.ToListAsync();
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

        public async Task ClearNationalParticipantsAsync()
        {
            using (var db = new CivlDataContext())
            {
                var participants = await db.NationParticipants.ToListAsync();
                db.NationParticipants.RemoveRange(participants);

                await db.SaveChangesAsync();
            }
        }
    }
}
