using System;
using System.Data;

namespace PGRating.Crawler.DataCollection
{
    public class TableFactory
    {
        public static DataColumn[] GetCompetitionColumns()
        {
            return new[]
            {
                new DataColumn("Periode",typeof(string)),
                new DataColumn("Name",typeof(string)),
                new DataColumn("CompetitionId",typeof(string)),
                new DataColumn("Discipline",typeof(string)),
                new DataColumn("Ta",typeof(string)),
                new DataColumn("Pn",typeof(string)),
                new DataColumn("Pq",typeof(string)),
                new DataColumn("Td",typeof(string)),
                new DataColumn("Tasks",typeof(string)),
                new DataColumn("Pq_srp",typeof(string)),
                new DataColumn("Pq_strp",typeof(string)),
                new DataColumn("PilotsNumber",typeof(string)),
                new DataColumn("PqRankingData",typeof(string)),
                new DataColumn("TotalPilots",typeof(string)),
                new DataColumn("TotalComps",typeof(string)),
                new DataColumn("DaysSinceCompetition",typeof(string)),
                new DataColumn("LastPilotScore",typeof(string)),
                new DataColumn("WinnerPilotScore",typeof(string)),
                new DataColumn("UpdateDate",typeof(string))
            };
        }

        internal static DataColumn[] GetNationPilotsColumns()
        {
            return new[]
            {
                new DataColumn("Rank",typeof(string)),
                new DataColumn("Name",typeof(string)),
                new DataColumn("PilotId",typeof(string)),
                new DataColumn("Gender",typeof(string)),
                new DataColumn("Nation",typeof(string)),
                new DataColumn("NationId",typeof(string)),
                new DataColumn("Points",typeof(string)),
                new DataColumn("CompetitionRating1",typeof(string)),
                new DataColumn("CompetitionId1",typeof(string)),
                new DataColumn("CompetitionRating2",typeof(string)),
                new DataColumn("CompetitionId2",typeof(string)),
                new DataColumn("CompetitionRating3",typeof(string)),
                new DataColumn("CompetitionId3",typeof(string)),
                new DataColumn("CompetitionRating4",typeof(string)),
                new DataColumn("CompetitionId4",typeof(string)),
                new DataColumn("Photo",typeof(string))
            };
        }
    }
}
