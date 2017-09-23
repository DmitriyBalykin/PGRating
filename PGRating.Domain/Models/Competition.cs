using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PGRating.Domain
{
    public class Competition
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }

        public double QualityCoefficient { get; set; }

        public double TimeCoefficient { get; set; }

        public List<Participant> Participants { get; set; }
    }
}