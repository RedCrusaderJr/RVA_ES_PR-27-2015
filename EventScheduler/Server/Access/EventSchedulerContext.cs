﻿using Common.Models;
using System;
using System.Collections.Generic;
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

        public DbSet<PersonWithAccount> PeopleWithAccounts { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}
