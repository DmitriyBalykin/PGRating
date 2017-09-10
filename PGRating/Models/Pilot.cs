using System.ComponentModel.DataAnnotations;

namespace PGRating.Models
{
    public class Pilot
    {
        [Key]
        public int Id { get; set; }

        public Nation Nation { get; set; }
    }
}