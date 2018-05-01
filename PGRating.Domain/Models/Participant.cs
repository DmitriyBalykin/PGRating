using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGRating.Domain
{
    public class Participant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Pilot Pilot { get; set; }

        public double Rating { get; set; }

        public DateTime RankingDate { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return this.Id.Equals(((Participant)obj).Id);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}