using PGRating.DAL.Repository;
using PGRating.Domain;
using PGRating.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Threading;
using System;
using PGRating.Crawler.Scrapper;

namespace PGRating.Scrapper.Scrapper
{
    public class DataScrapper
    {
        //Run once in a week
        private static readonly TimeSpan RunningInterval = TimeSpan.FromDays(7);

        public static void Start()
        {
            HostingEnvironment.QueueBackgroundWorkItem(BackgroundWorker);            
        }

        private static async Task BackgroundWorker(CancellationToken obj)
        {
            //var competitions = await GetActualCompetitions();
            //await SaveActualCompetitions(competitions);

            await RegularTasks.Run();

            await Task.Delay(RunningInterval);
        }

        private static async Task<List<Competition>> GetActualCompetitions()
        {
            var dataCalculator = new RatingTableCalculator();
            var competitionsDictionary = await dataCalculator.GetCompetitionsAsync();

            return competitionsDictionary.Values.ToList();
        }

        private static async Task SaveActualCompetitions(List<Competition> competitions)
        {
            try
            {
                var repository = new CompetitionsRepository();

                await repository.SaveCompetitions(competitions);
            }
            catch (System.Exception ex)
            {

            }

        }
    }
}
