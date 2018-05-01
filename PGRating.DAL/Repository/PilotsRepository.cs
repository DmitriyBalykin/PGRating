using PGRating.DAL.DataContext;
using PGRating.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace PGRating.DAL.Repository
{
    public class PilotsRepository: IDisposable
    {
        private bool isDisposing = false;
        private CivlDataContext datacontext;
        public PilotsRepository()
        {
            this.datacontext = new CivlDataContext();
        }

        public Task<List<Pilot>> GetPilotsAsync()
        {
            var pilots = this.datacontext.Pilots.Include(p => p.Nation);
            return pilots.ToListAsync();
        }

        public async Task SavePilotsAsync(IList<Pilot> pilots)
        {
            this.datacontext.Pilots.AddRange(pilots);

            await this.datacontext.SaveChangesAsync();
        }

        public async Task ClearPilotsAsync()
        {
            var pilots = await this.datacontext.Pilots.Include(p => p.Nation).ToListAsync();
            this.datacontext.Pilots.RemoveRange(pilots);

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
