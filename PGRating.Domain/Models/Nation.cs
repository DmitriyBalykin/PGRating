using System.ComponentModel.DataAnnotations;

namespace PGRating.Domain
{
    public class Nation
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return this.Id == ((Nation)obj).Id;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}