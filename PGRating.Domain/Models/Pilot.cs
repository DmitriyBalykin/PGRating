using System;
using System.ComponentModel.DataAnnotations;

namespace PGRating.Domain
{
    public class Pilot
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual Nation Nation { get; set; }

        public decimal Rating { get; set; }

        public DateTime RatingDate { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            return this.Id.Equals(((Pilot)obj).Id);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            
            return this.Id;
        }
    }
}