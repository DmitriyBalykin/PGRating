using PGRating.Crawler.DataCollection;
using PGRating.Crawler.Loader;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using System;
using System.Threading;
using PGRating.Domain;

namespace PGRating.Utilities
{
    public class RatingTableCalculator
    {
        private static readonly char decimalDelimiter = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);


        public async Task<Dictionary<int, Competition>> GetCompetitionsAsync()
        {
            var loader = new WebLoader();

            var actualCompetitionsCrawler = new ActualCompetitionsDataReader(loader);

            var executionRootPath = Path.GetFullPath(HostingEnvironment.ApplicationPhysicalPath);

            var projectRootPath = Directory.GetParent(Directory.GetParent(executionRootPath).FullName).FullName;

            //var competitionsDataPath = $"{projectRootPath}\\SampleData\\AllCopmetitionsPageData.txt";
            //var participantsDataPath = $"{projectRootPath}\\SampleData\\UkrainianPilotsTable.txt";

            var competitionsDataPath = $"http://civlrankings.fai.org/?a=327&ladder_id=3&ranking_date=2017-09-01&";
            var competitionsTable = await actualCompetitionsCrawler.LoadUsedCompetitionsTableAsync(competitionsDataPath);


            var competitions = new Dictionary<int, Competition>();

            foreach (DataRow competitionRow in competitionsTable.Rows)
            {
                var competitionId = GetCellValue(competitionRow, "CompetitionId");
                var pq = GetCellValue(competitionRow, "Pq");
                var td = GetCellValue(competitionRow, "Td");
                var name = GetCellValue(competitionRow, "Name");
                int parsedId = GeInt(competitionId);

                if (competitions.ContainsKey(parsedId))
                {
                    continue;
                }

                var parsedPq = GetDouble(pq);
                var parsedTd = GetDouble(td);

                competitions.Add(
                    parsedId,
                    new Competition
                    {
                        Id = parsedId,
                        Name = name.ToString(),
                        QualityCoefficient = parsedPq,
                        TimeCoefficient = parsedTd
                    });
            }

            return competitions;
        }

        public async Task<List<NationTeamParticipant>> GetParticipantsAsync()
        {
            var loader = new WebLoader();

            var nationPilotsCrawler = new NationPilotsDataReader(loader);
            var participantsDataPath = $"http://civlrankings.fai.org/?a=326&ladder_id=3&ranking_date=2017-09-01&nation_id=230&";
            var participantsTable = await nationPilotsCrawler.LoadNationPilotsTableAsync(participantsDataPath);

            var participants = new List<NationTeamParticipant>();

            var competitions = await this.GetCompetitionsAsync();

            foreach (DataRow participantRow in participantsTable.Rows)
            {
                var rank = GetCellValue(participantRow, "Rank").ToString();
                rank = rank.Substring(0, rank.IndexOf('w'));

                var name = GetCellValue(participantRow, "Name").ToString();
                name = name.Substring(0, name.IndexOf("CIVL", StringComparison.InvariantCultureIgnoreCase));

                var rating = GetCellValue(participantRow, "Points");

                var cr1 = GetCellValue(participantRow, "CompetitionRating1");
                var cr2 = GetCellValue(participantRow, "CompetitionRating2");
                var cr3 = GetCellValue(participantRow, "CompetitionRating3");
                var cr4 = GetCellValue(participantRow, "CompetitionRating4");

                var cid1 = GetCellValue(participantRow, "CompetitionId1");
                var cid2 = GetCellValue(participantRow, "CompetitionId2");
                var cid3 = GetCellValue(participantRow, "CompetitionId3");
                var cid4 = GetCellValue(participantRow, "CompetitionId4");

                var cid1Parsed = GeInt(cid1);
                var cid2Parsed = GeInt(cid2);
                var cid3Parsed = GeInt(cid3);
                var cid4Parsed = GeInt(cid4);

                var cq1 = GetCq(competitions, cid1Parsed);
                var cq2 = GetCq(competitions, cid2Parsed);
                var cq3 = GetCq(competitions, cid3Parsed);
                var cq4 = GetCq(competitions, cid4Parsed);

                var cr1Parsed = GetDouble(cr1);
                var cr2Parsed = GetDouble(cr2);
                var cr3Parsed = GetDouble(cr3);
                var cr4Parsed = GetDouble(cr4);

                var rankParsed = GeInt(rank);
                var ratingParsed = GetDouble(rating);

                participants.Add(
                    new NationTeamParticipant
                    {
                        Rank = rankParsed,
                        Name = name,
                        Rating = ratingParsed,
                        CQ1 = cq1,
                        CQ2 = cq2,
                        CQ3 = cq3,
                        CQ4 = cq4,
                        CR1 = cr1Parsed,
                        CR2 = cr2Parsed,
                        CR3 = cr3Parsed,
                        CR4 = cr4Parsed
                    });
            }

            return participants;
        }

        private static double GetCq(Dictionary<int, Competition> competitions, int key)
        {
            if (competitions.ContainsKey(key))
            {
                return competitions[key].QualityCoefficient;
            }

            return 0;
        }

        private static object GetCellValue(DataRow participantRow, string key)
        {
            try
            {
                return participantRow[key];
            }
            catch (KeyNotFoundException ex)
            {
                return null;
            }
        }

        private static int GeInt(object value)
        {
            if (value == null)
            {
                return 0;
            }

            var str = value.ToString();
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }

            try
            {
                return int.Parse(value.ToString());
            }
            catch (FormatException ex)
            {

                throw;
            }
            
        }

        private static double GetDouble(object value)
        {
            if (value == null)
            {
                return 0;
            }
            var str = value.ToString();
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }

            try
            {
                return double.Parse(value.ToString().Replace('.', decimalDelimiter));
            }
            catch (FormatException ex)
            {

                throw;
            }
        }
    }
}