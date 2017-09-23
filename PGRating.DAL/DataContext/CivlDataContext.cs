using PGRating.Domain;
using System.Data.Entity;

namespace PGRating.DAL.DataContext
{
    internal class CivlDataContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public CivlDataContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<Competition> Competitions { get; set; }

        public DbSet<Pilot> Pilots { get; set; }

        public DbSet<Participant> Participants { get; set; }

        public DbSet<NationTeamParticipant> NationParticipants { get; set; }
    }
}
