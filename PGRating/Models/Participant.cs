using System.ComponentModel.DataAnnotations;

namespace PGRating.Models
{
    public class Participant
    {
        [Key]
        public int Id { get; set; }

        public Pilot Pilot { get; set; }

        public Competition Competition { get; set; }

        public double Rating { get; set; }
    }
}