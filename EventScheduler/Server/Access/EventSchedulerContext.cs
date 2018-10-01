using Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Access
{
    public class EventSchedulerContext : DbContext
    {
        public EventSchedulerContext() : base("esDbConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EventSchedulerContext, Configuration>());
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasMany(e => e.Participants)
                                        .WithMany(p => p.ScheduledEvents)
                                        .Map(m => {
                                            m.MapLeftKey("EventId");
                                            m.MapRightKey("JMBG");
                                            m.ToTable("EventPerson");
                                        });
            base.OnModelCreating(modelBuilder);
        }
    }   
}
