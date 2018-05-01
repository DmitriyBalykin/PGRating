using PGRating.Crawler.DataCollection;
using PGRating.Crawler.Loader;
using PGRating.DAL.Repository;
using PGRating.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PGRating.Crawler.Scrapper
{
    public class RegularTasks
    {
        private static readonly TimeSpan ExecutionInterval = TimeSpan.FromSeconds(5);
        private ILoader loader;

        public RegularTasks(ILoader loader)
        {
            this.loader = loader;
        }

        public async Task Run()
        {
            await UpdateNationsTask();
            await UpdatePilotsRatingTask();
        }

        public async Task UpdateNationsTask()
        {
            var dataReader = new NationsDataReader(this.loader);
            var nationsList = await dataReader.LoadNationsAsync();
            var repository = new NationsRepository();
            var existingNations = (await repository.GetNationsAsync()).OrderBy(nation => nation.Id);
            var changedNations = nationsList.OrderBy(nation => nation.Id).Except(existingNations).ToList();

            if(changedNations.Count > 0)
            {
                await repository.SaveNationsAsync(changedNations);
            }
        }

        public async Task UpdatePilotsRatingTask()
        {
            var nations = await new NationsRepository().GetNationsAsync();
            // Load all pilots list 
            var dataReader = new NationPilotsDataReader(this.loader);

            using (var repository = new PilotsRepository())
            {
                foreach (var nation in nations)
                {
                    var pilots = await dataReader.LoadNationPilotsAsync(nation.Id);
                    // Save into repository
                    await repository.SavePilotsAsync(pilots);

                    await Task.Delay(ExecutionInterval);
                }
            }
        }
    }
}
