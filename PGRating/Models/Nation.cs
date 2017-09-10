using System.ComponentModel.DataAnnotations;

namespace PGRating.Models
{
    public class Nation
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}