using System.Collections.Generic;
namespace PGRating.Models
{
    public class TablesCombinationModel
    {
        public List<NationTeamParticipant> DirectList { get; set; }
        public List<NationTeamParticipant> EquivalentList { get; set; }
    }
}