using PGRating.DAL.DataContext;
using PGRating.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PGRating.DAL.Repository
{
    public class CivlDataRepository
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
