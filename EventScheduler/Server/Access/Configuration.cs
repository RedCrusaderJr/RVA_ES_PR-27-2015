using Common.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Access
{
    class Configuration : DbMigrationsConfiguration<EventSchedulerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Context";
        }

        protected override void Seed(EventSchedulerContext context)
        {
            Person admin = new Person("admin")
            {
                FirstName = "Pera",
                LastName = "Peric",
                Password = "admin",
                Role = ERole.ADMIN
            };

            DBManager.Instance.AddPerson(admin);
        }
    }
}
