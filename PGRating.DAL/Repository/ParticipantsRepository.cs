using PGRating.DAL.DataContext;
using PGRating.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGRating.DAL.Repository
{
    public class ParticipantsRepository
    {
        public List<Participant> GetParticipants()
        {
            using (var db = new CivlDataContext())
            {
                return db.Participants.ToList();
            }
        }

        public async Task SaveParticipants(List<Participant> participants)
        {
            using (var db = new CivlDataContext())
            {
                db.Participants.AddRange(participants);

                await db.SaveChangesAsync();
            }
        }
    }
}
