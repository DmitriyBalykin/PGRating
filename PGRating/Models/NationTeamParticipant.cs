using System.ComponentModel;

namespace PGRating.Models
{
    public class NationTeamParticipant
    {
        [DisplayName("Rank")]
        public int Rank { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("FAI Rating")]
        public double Rating { get; set; }

        [DisplayName("CR1")]
        public double CR1 { get; set; }

        [DisplayName("CQ1")]
        public double CQ1 { get; set; }

        [DisplayName("CR2")]
        public double CR2 { get; set; }

        [DisplayName("CQ2")]
        public double CQ2 { get; set; }

        [DisplayName("CR3")]
        public double CR3 { get; set; }

        [DisplayName("CQ3")]
        public double CQ3 { get; set; }

        [DisplayName("CR4")]
        public double CR4 { get; set; }

        [DisplayName("CQ4")]
        public double CQ4 { get; set; }

        [DisplayName("Equivalent Rating")]
        public double EquivalentRating { get; set; }
    }
}