using HtmlAgilityPack;
using PGRating.Crawler.Loader;
using PGRating.Crawler.Utilities;
using PGRating.Domain;
using PGRating.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PGRating.Crawler.DataCollection
{
    public class NationsDataReader
    {
        private const string BaseUrl = @"http://civlrankings.fai.org/?a=328&ladder_id=3";
        private const string NationIdKey = "nation_id";

        private static List<string> ColumnHeaders = new List<string>
        {
            "Rank",
            "Nation",
            "Pilots",
            "Points",
            "",
            "Photo"
        };

        private ILoader loader;

        public NationsDataReader(ILoader loader)
        {
            this.loader = loader;
        }

        public async Task<IList<Nation>> LoadNationsAsync(string url = null)
        {

            var htmlTable = await this.LoadPageTableNodeAsync(url);

            var dataTable = GetNationsFromHtml(htmlTable);

            return dataTable;
        }

        private static IList<Nation> GetNationsFromHtml(HtmlNode htmlTable)
        {
            VerifyTable(htmlTable);

            var list = new List<Nation>();

            foreach (var row in htmlTable.SelectNodes("tr"))
            {
                var cells = row.SelectNodes("td");

                if (cells == null)
                {
                    continue;
                }

                string nationCellText = cells[ColumnHeaders.IndexOf("Nation")].InnerHtml;
                // <a href='?a=326&ladder_id=3&ranking_date=2017-11-01&nation_id=81&'>France</a>
                var attributes = ParseUtilities.GetTagAttributes(nationCellText, "a");
                var nationId = ParseUtilities.GetValueFromUrl(attributes, NationIdKey);
                var nationName = ParseUtilities.GetTagContent(nationCellText, "a");
                list.Add(
                    new Nation
                    {
                        Id = ParseUtilities.GetInt(nationId),
                        Name = nationName
                    });
            }

            return list;
        }

        private static void VerifyTable(HtmlNode htmlTable)
        {
            foreach (var row in htmlTable.SelectNodes("tr"))
            {
                var cells = row.SelectNodes("th");

                if (cells == null)
                {
                    throw new InvalidTableException();
                }

                var headersAreCorrect = cells.Zip(ColumnHeaders, (cell, header) => new { cellHeader = cell.InnerHtml, referenceHeader = header })
                    .All(item => item.cellHeader.Equals(item.referenceHeader, StringComparison.InvariantCultureIgnoreCase));

                if (headersAreCorrect)
                {
                    return;
                }
                else
                {
                    throw new InvalidTableException();
                }
            }
        }

        private async Task<HtmlNode> LoadPageTableNodeAsync(string url = null)
        {
            url = string.IsNullOrEmpty(url) ? BaseUrl : url;
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
