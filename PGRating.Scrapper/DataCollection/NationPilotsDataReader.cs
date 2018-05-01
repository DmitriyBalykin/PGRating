using HtmlAgilityPack;
using PGRating.Crawler.Loader;
using PGRating.Crawler.Utilities;
using PGRating.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PGRating.Crawler.DataCollection
{
    public class NationPilotsDataReader
    {
        private const string BaseUsl = @"http://civlrankings.fai.org/?a=326&ladder_id=3";
        private const string CompetitionsListPage = @"http://civlrankings.fai.org/?a=327&ladder_id=3";
        private const string NationPilotsListPage = @"http://civlrankings.fai.org/?a=326&ladder_id=3";

        private const string CompetitionIdKey = "competition_id=";
        private const string NationIdKey = "nation_id";
        private const string PersonIdKey = "person_id";
        private const string DateKey = "ranking_date";

        private const int CountedCompetitionsPerPilots = 4;

        //Do not change the order!!!
        private static readonly HashSet<string> IdKeys = new HashSet<string>
        {
            CompetitionIdKey,
            NationIdKey,
            PersonIdKey
        };

        private ILoader loader;

        public NationPilotsDataReader(ILoader loader)
        {
            this.loader = loader;
        }

        public async Task<IList<Pilot>> LoadNationPilotsAsync(int nationId = 0, string url = null)
        {
            var table = await this.LoadNationPilotsTableAsync(nationId, url);

            var list = new List<Pilot>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(new Pilot
                {
                    Id = int.Parse(row[2].ToString()),
                    Name = row[3].ToString(),
                    Nation = new Nation
                    {
                        Name = row[5].ToString(),
                        Id = int.Parse(row[6].ToString()),
                    }
                });
            }

            return list;
        }

        public async Task<DataTable> LoadNationPilotsTableAsync(int nationId = 0, string url = null)
        {
            var dataTable = new DataTable("NationPilots");

            dataTable.Columns.AddRange(TableFactory.GetNationPilotsColumns());

            var htmlTable = await this.LoadNationPilotsPageAsync(nationId, url);

            PopulateTableFromHtml(dataTable, htmlTable);

            return dataTable;
        }

        private static void PopulateTableFromHtml(DataTable dataTable, HtmlNode htmlTable)
        {

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
                    if (cell.InnerHtml.Contains("Ranking points for the competition in this ranking"))
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
                        index = ExtractNameIfExistsAndInsertNext(dataRow, index, text);
                    }
                }

                dataTable.Rows.Add(dataRow);
            }
        }

        private static int ExtractNameIfExistsAndInsertNext(DataRow dataRow, int index, string text)
        {
            var regex = new Regex("^([a-zA-Z ]+)CIVL ID: ([0-9]+)");
            var name = ParseUtilities.GetMatches(text, regex)?.FirstOrDefault();

            if (!string.IsNullOrEmpty(name))
            {
                index = InsertNext(dataRow, index, name);
            }

            return index;
        }

        private static IEnumerable<Tuple<string,string>> ExtractAllCompetitionIds(string innerHtml)
        {
            var competitionDataList = innerHtml.Split(new[] { "<br>" }, 4, StringSplitOptions.RemoveEmptyEntries);

            var listHasManyItems = competitionDataList.Length > 1;

            foreach (var competitionData in competitionDataList)
            {
                var parts = competitionData.Split(' ');
                var rating = parts[1];
                string competitionId;

                if (listHasManyItems)
                {
                    competitionId = ExctractId(parts[3], CompetitionIdKey);
                }
                else
                {
                    competitionId = ExctractId(innerHtml, CompetitionIdKey);
                }

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

            return ParseUtilities.GetValueFromUrl(value, key);
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
            value = ParseUtilities.Trim(value);
            dataRow.SetField<string>(index, value);
            index++;

            return index;
        }

        public async Task<HtmlNode> LoadUsedCompetitionsPageAsync(string url = null)
        {
            if (url == null)
            {
                url = $"{CompetitionsListPage}" +
                    $"&{DateKey}={DateTime.Now.ToString("yyyy-MM-01")}";
            }

            return await this.LoadPageTableNodeAsync(url);
        }

        private async Task<HtmlNode> LoadNationPilotsPageAsync(int nationId=0, string url = null)
        {
            if (url == null)
            {
                url = $"{NationPilotsListPage}" +
                    $"&{DateKey}={DateTime.Now.ToString("yyyy-MM-01")}" +
                    $"&{NationIdKey}={nationId}";
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
