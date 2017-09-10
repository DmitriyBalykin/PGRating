using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PGRating.Models
{
    public class Competition
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public double Quality { get; set; }

        public List<Pilot> Participants { get; set; }
    }
}