using System;
using System.ComponentModel.DataAnnotations;

namespace PGRating.Models
{
    public class RatingTableViewModel
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Data { get; set; }
    }
}