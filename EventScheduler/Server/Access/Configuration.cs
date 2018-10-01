using Common;
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
            //Person adminPerson = new Person("006", "Pera", "Peric");
            Account admin = new Account("admin", "admin", ERole.ADMIN)
            {
                FirstName = "Pera",
                LastName = "Peric",
            };
            DbManager.Instance.AddAccount(admin);
        }
    }
}
