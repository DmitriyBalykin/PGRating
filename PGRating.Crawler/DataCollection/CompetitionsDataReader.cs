using HtmlAgilityPack;
using PGRating.Crawler.Loader;
using System;
using System.Data;
using System.Threading.Tasks;

namespace PGRating.Crawler.DataCollection
{
    public class CompetitionsDataReader
    {
        private const string BaseUsl = @"http://civlrankings.fai.org/?a=326&ladder_id=3";
        private const string RankingDatePart = "&ranking_date=2017-09-01";
        private const string NationIdPart = "&nation_id=230";
        private const string CompetitionsListPage = @"http://civlrankings.fai.org/?a=327&ladder_id=3&ranking_date=";

        private const string CompetitionIdString = "competition_id";

        private ILoader loader;

        public CompetitionsDataReader(ILoader loader)
        {
            this.loader = loader;
        }

        public async Task<DataTable> LoadUsedCompetitionsTableAsync(string url = null)
        {
            var dataTable = new DataTable("UsedCompetitions");

            dataTable.Columns.AddRange(new[] 
            {
                new DataColumn("Periode",typeof(string)),
                new DataColumn("Name",typeof(string)),
                new DataColumn("Id",typeof(string)),
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
            });

            var htmlTable = await this.LoadUsedCompetitionsPageAsync(url);

            foreach (var row in htmlTable.SelectNodes("tr"))
            {
                var dataRow = dataTable.NewRow();

                dataTable.Rows.Add(dataRow);

                var cells = row.SelectNodes("th|td");

                int index = 0;

                foreach (var cell in cells)
                {
                    var text = cell.InnerText;

                    if (string.IsNullOrEmpty(text))
                    {
                        continue;
                    }

                    index = InsertNext(dataRow, index, text);

                    if (cell.Name == "td")
                    {
                        var competitionId = ExctractCompetitionId(cell.InnerHtml);

                        if (competitionId != null)
                        {
                            index = InsertNext(dataRow, index, competitionId);
                        }
                    }
                }
            }

            return dataTable;
        }

        private static string ExctractCompetitionId(string value)
        {

            var keyStringIndex = value.IndexOf(CompetitionIdString);

            if (keyStringIndex > -1)
            {
                var idStringStart = value.IndexOf('=', keyStringIndex) + 1;
                var idStringLength = value.IndexOf('&', keyStringIndex) - idStringStart;

                if (idStringStart != -1 && idStringLength > 0)
                {
                    var idString = value.Substring(idStringStart, idStringLength);

                    return idString;
                }
            }

            return null;
        }

        private static int InsertNext(DataRow dataRow, int index, string value)
        {
            dataRow.SetField<string>(index, value);
            index++;

            return index;
        }

        public async Task<HtmlNode> LoadUsedCompetitionsPageAsync(string url = null)
        {
            var scoringDate = DateTime.Now.ToString("yyyy-MM-01");

            if (url == null)
            {
                url = CompetitionsListPage + scoringDate;
            }

            return await this.LoadPageTableNodeAsync(url);
        }

        private async Task<HtmlNode> LoadPageTableNodeAsync(string url)
        {

            try
            {
                var documentNode = await Task.Run(() =>
                {
                    this.loader.Load(url);

                    return this.loader.DocumentNode;
                });

                var tableNode = documentNode.SelectSingleNode("//body/form/table");

                return tableNode;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

    }
}
