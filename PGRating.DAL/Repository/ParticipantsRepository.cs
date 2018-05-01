using PGRating.DAL.DataContext;
using PGRating.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PGRating.DAL.Repository
{
    public class ParticipantsRepository : IDisposable
    {
        private bool isDisposing;
        private CivlDataContext datacontext;
        public ParticipantsRepository()
        {
            this.datacontext = new CivlDataContext();
        }

        public Task<List<Participant>> GetParticipantsAsync()
        {
            return this.datacontext.Participants.Include(p => p.Pilot).ToListAsync();
        }

        public async Task SaveParticipants(List<Participant> participants)
        {
            this.datacontext.Participants.AddRange(participants);

            await this.datacontext.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (this.isDisposing)
            {
                return;
            }

            this.isDisposing = true;
            this.datacontext.Dispose();
        }
    }
}
