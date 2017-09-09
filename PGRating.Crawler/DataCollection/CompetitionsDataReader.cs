using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGRating.Crawler.DataCollection
{
    public class CompetitionsDataReader
    {
        private const string BaseUsl = @"http://civlrankings.fai.org/?a=326&ladder_id=3";
        private const string RankingDatePart = "&ranking_date=2017-09-01";
        private const string NationIdPart = "&nation_id=230";
    }
}
