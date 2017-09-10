using HtmlAgilityPack;
using PGRating.Crawler.Loader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PGRating.Crawler.DataCollection
{
    public class CompetitionsDataReader
    {
        private const string BaseUsl = @"http://civlrankings.fai.org/?a=326&ladder_id=3";
        private const string RankingDatePart = "&ranking_date=2017-09-01";
        private const string NationIdPart = "&nation_id=230";
        private const string CompetitionsListPage = @"http://civlrankings.fai.org/?a=327&ladder_id=3&ranking_date=";
        private const string NationPilotsListPage = @"http://civlrankings.fai.org/?a=326&ladder_id=3&ranking_date=";

        private const string CompetitionIdKey = "competition_id=";
        private const string NationIdKey = "nation_id=";
        private const string PersonIdKey = "person_id=";

        private const int CountedCompetitionsPerPilots = 4;

        //Do not change the order!!!
        private static readonly HashSet<string> IdKeys = new HashSet<string>
        {
            CompetitionIdKey,
            NationIdKey,
            PersonIdKey
        };

        private ILoader loader;

        public CompetitionsDataReader(ILoader loader)
        {
            this.loader = loader;
        }

        public async Task<DataTable> LoadUsedCompetitionsTableAsync(string url = null)
        {
            var dataTable = new DataTable("UsedCompetitions");

            dataTable.Columns.AddRange(TableFactory.GetCompetitionColumns());

            var htmlTable = await this.LoadUsedCompetitionsPageAsync(url);

            PopulateTableFromHtml(dataTable, htmlTable);

            return dataTable;
        }

        public async Task<DataTable> LoadNationPilotsTableAsync(string url = null)
        {
            var dataTable = new DataTable("NationPilots");

            dataTable.Columns.AddRange(TableFactory.GetNationPilotsColumns());

            var htmlTable = await this.LoadNationPilotsPageAsync(url);

            PopulateTableFromHtml(dataTable, htmlTable);

            return dataTable;
        }

        private static void PopulateTableFromHtml(DataTable dataTable, HtmlNode htmlTable)
        {
            var competitionIdRegex = new Regex(CompetitionIdKey);

            foreach (var row in htmlTable.SelectNodes("tr"))
            {
                var dataRow = dataTable.NewRow();

                var cells = row.SelectNodes("td");

                if (cells == null)
                {
                    continue;
                }

                int index = 0;

                foreach (var cell in cells)
                {
                    if (competitionIdRegex.Matches(cell.InnerHtml).Count == CountedCompetitionsPerPilots)
                    {
                        var competitionNameIdPairs = ExtractAllCompetitionIds(cell.InnerHtml);

                        foreach (var pair in competitionNameIdPairs)
                        {
                            index = InsertNext(dataRow, index, pair.Item1);
                            index = InsertNext(dataRow, index, pair.Item2);
                        }
                    }
                    else
                    {
                        var text = cell.InnerText;

                        if (string.IsNullOrEmpty(text))
                        {
                            continue;
                        }

                        index = InsertNext(dataRow, index, text);

                        index = ExtractIdIfExistsAndInsertNext(dataRow, index, cell.InnerHtml);
                    }
                }

                dataTable.Rows.Add(dataRow);
            }
        }

        private static IEnumerable<Tuple<string,string>> ExtractAllCompetitionIds(string innerHtml)
        {
            var competitionDataList = innerHtml.Split(new[] { "<br>" }, 4, StringSplitOptions.RemoveEmptyEntries);

            foreach (var competitionData in competitionDataList)
            {
                var parts = competitionData.Split(' ');
                var rating = parts[1];
                var competitionId = ExctractId(parts[3]);

                yield return new Tuple<string, string>(rating, competitionId);
            }
        }

        private static int ExtractIdIfExistsAndInsertNext(DataRow dataRow, int index, string innerHtml)
        {
            var Id = ExctractId(innerHtml);

            if (Id != null)
            {
                index = InsertNext(dataRow, index, Id);
            }

            return index;
        }

        private static string ExctractId(string value, string key = null)
        {
            if (key == null)
            {
                key = SelectIdKey(value);
            }

            if (key == null)
            {
                return null;
            }

            var keyStringIndex = value.IndexOf(key);

            if (keyStringIndex > -1)
            {
                var idStringStart = keyStringIndex + key.Length;
                value = value.Substring(idStringStart);

                var regex = new Regex("^[0-9]+");

                var match = regex.Match(value);

                if (match.Success)
                {
                    return match.Groups[0].Value;
                }
            }

            return null;
        }

        private static string SelectIdKey(string value)
        {
            foreach (var key in IdKeys)
            {
                if (value.Contains(key))
                {
                    return key;
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
            if (url == null)
            {
                var scoringDate = DateTime.Now.ToString("yyyy-MM-01");
                url = CompetitionsListPage + scoringDate;
            }

            return await this.LoadPageTableNodeAsync(url);
        }

        private async Task<HtmlNode> LoadNationPilotsPageAsync(string url = null)
        {
            if (url == null)
            {
                var scoringDate = DateTime.Now.ToString("yyyy-MM-01");
                url = NationPilotsListPage + scoringDate + NationIdPart;
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

                var tableNode = documentNode.SelectSingleNode("//table");

                return tableNode;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

    }
}
