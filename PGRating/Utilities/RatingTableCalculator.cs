using PGRating.Crawler.DataCollection;
using PGRating.Crawler.Loader;
using PGRating.Models;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using System;
using System.Linq;
using System.Threading;

namespace PGRating.Utilities
{
    public class RatingTableCalculator
    {
        private static readonly char decimalDelimiter = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

        public async Task<TablesCombinationModel> GetParticipantsAsync()
        {
            var loader = new WebLoader();

            var crawler = new CompetitionsDataReader(loader);

            var executionRootPath = Path.GetFullPath(HostingEnvironment.ApplicationPhysicalPath);

            var projectRootPath = Directory.GetParent(Directory.GetParent(executionRootPath).FullName).FullName;

            //var competitionsDataPath = $"{projectRootPath}\\SampleData\\AllCopmetitionsPageData.txt";
            //var participantsDataPath = $"{projectRootPath}\\SampleData\\UkrainianPilotsTable.txt";

            var competitionsDataPath = $"http://civlrankings.fai.org/?a=327&ladder_id=3&ranking_date=2017-09-01&";
            var participantsDataPath = $"http://civlrankings.fai.org/?a=326&ladder_id=3&ranking_date=2017-09-01&nation_id=230&";

            var competitionsTable = await crawler.LoadUsedCompetitionsTableAsync(competitionsDataPath);
            var participantsTable = await crawler.LoadNationPilotsTableAsync(participantsDataPath);

            var competitions = new Dictionary<int, Competition>();
            
            foreach (DataRow competitionRow in competitionsTable.Rows)
            {
                var competitionId = GetCellValue(competitionRow,"CompetitionId");
                var pq = GetCellValue(competitionRow, "Pq");
                var name = GetCellValue(competitionRow, "Name");
                int parsedId = GeInt(competitionId);
                
                if (competitions.ContainsKey(parsedId))
                {
                    continue;
                }

                var parsedPq = GetDouble(pq);

                competitions.Add(
                    parsedId,
                    new Competition
                    {
                        Id = parsedId,
                        Name = name.ToString(),
                        Quality = parsedPq
                    });
            }

            var participants = new List<NationTeamParticipant>();

            foreach (DataRow participantRow in participantsTable.Rows)
            {
                var rank = GetCellValue(participantRow, "Rank").ToString();
                rank = rank.Substring(0, rank.IndexOf('w'));

                var name = GetCellValue(participantRow, "Name").ToString();
                name = name.Substring(0, name.IndexOf("CIVL"));

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
                        Name = name.ToString(),
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

            CalculateQuivalentRankings(participants);

            var equivalentRatingOrderList = participants.OrderByDescending(part => part.EquivalentRating).ToList();

            var model = new TablesCombinationModel
            {
                DirectList = participants,
                EquivalentList = equivalentRatingOrderList
            };

            return model;
        }

        private static double GetCq(Dictionary<int, Competition> competitions, int key)
        {
            if (competitions.ContainsKey(key))
            {
                return competitions[key].Quality;
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

        private void CalculateQuivalentRankings(List<NationTeamParticipant> participants)
        {
            foreach (var part in participants)
            {
                part.EquivalentRating = part.CR1 * part.CQ1 + part.CR2 * part.CQ2 + part.CR3 * part.CQ3 + part.CR4 * part.CQ4;
            }
        }
    }
}