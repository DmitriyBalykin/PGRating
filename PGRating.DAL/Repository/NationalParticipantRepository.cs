using PGRating.DAL.DataContext;
using PGRating.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace PGRating.DAL.Repository
{
    public class NationalParticipantRepository: IDisposable
    {
        private bool isDisposed;
        private CivlDataContext datacontext;

        public NationalParticipantRepository()
        {
            this.datacontext = new CivlDataContext();
        }
        public Task<List<NationTeamParticipant>> GetNationalParticipantsAsync()
        {
            return this.datacontext.NationParticipants.ToListAsync();
        }

        public async Task SaveNationalParticipants(IList<NationTeamParticipant> participants)
        {
            this.datacontext.NationParticipants.AddRange(participants);

            await this.datacontext.SaveChangesAsync();
        }

        public async Task ClearNationalParticipantsAsync()
        {
            var participants = await this.datacontext.NationParticipants.ToListAsync();
            this.datacontext.NationParticipants.RemoveRange(participants);

            await this.datacontext.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (this.isDisposed)
            {
                return;
            }

            this.isDisposed = true;
            this.datacontext.Dispose();
        }
    }
}
