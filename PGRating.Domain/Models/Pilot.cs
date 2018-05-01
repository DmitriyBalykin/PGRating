using System.ComponentModel.DataAnnotations;

namespace PGRating.Domain
{
    public class Pilot
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual Nation Nation { get; set; }
    }
}