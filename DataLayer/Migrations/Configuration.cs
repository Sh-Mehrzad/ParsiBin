using ParsiBin.DataLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsiBin.DataLayer
{
    public class Configuration : DbMigrationsConfiguration<ParsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ParsContext context)
        {
            //var employee1 = new Employee { FirstName = "f name1", LastName = "l name1" };
            //var employee2 = new Employee { FirstName = "f name2", LastName = "l name2" };
            //var employee3 = new Employee { FirstName = "f name3", LastName = "l name3" };
            //var employee4 = new Employee { FirstName = "f name4", LastName = "l name4" };

            //var dept1 = new Department { Name = "dept 1", Employees = new List<Employee> { employee1, employee2 } };
            //var dept2 = new Department { Name = "dept 2", Employees = new List<Employee> { employee3 } };
            //var dept3 = new Department { Name = "dept 3", Employees = new List<Employee> { employee4 } };

            //context.Departments.Add(dept1);
            //context.Departments.Add(dept2);
            //context.Departments.Add(dept3);
            base.Seed(context);
        }
    }
}
