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
        private static readonly TimeSpan ExecutionInterval = TimeSpan.FromMinutes(5);
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
            var existingNations = await repository.GetNationsAsync();
            var changedNations = nationsList.Except(existingNations).ToList();

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

            foreach(var nation in nations)
            {
                var participantsTable = await dataReader.LoadNationPilotsTableAsync(nation.Id);
                // Convert to pilot objects list
                var participantsList = new List<NationTeamParticipant>();
                // Save into repository
                var repository = new NationalParticipantRepository();
                await repository.SaveNationalParticipants(participantsList);

                await Task.Delay(ExecutionInterval);
            }
        }
    }
}
