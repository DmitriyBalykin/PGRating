using PGRating.DAL.Repository;
using PGRating.Domain;
using PGRating.Models;
using System.Collections.Generic;
using System.Linq;

namespace PGRating.Utils
{
    public class ViewModelHelper
    {
        public static TablesCombinationModel GetRatingCombinationTableAsync()
        {
            var repository = new NationalParticipantRepository();

            var participants = repository.GetNationalParticipants();


            CalculateEquivalentRankings(participants);

            var equivalentRatingOrderList = participants.OrderByDescending(part => part.EquivalentRating).ToList();

            var model = new TablesCombinationModel
            {
                DirectList = participants,
                EquivalentList = equivalentRatingOrderList
            };

            return model;
        }

        private static void CalculateEquivalentRankings(List<NationTeamParticipant> participants)
        {
            foreach (var part in participants)
            {
                part.EquivalentRating = part.CR1 * part.CQ1 + part.CR2 * part.CQ2 + part.CR3 * part.CQ3 + part.CR4 * part.CQ4;
            }
        }
    }
}