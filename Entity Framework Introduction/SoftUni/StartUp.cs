using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SoftUni.Data;
using SoftUni.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext db= new SoftUniContext();
            var result = RemoveTown(db);
            
            Console.WriteLine(result);
        }

        //Problem 03
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb= new StringBuilder();

            var employees = context.Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary,
                    e.EmployeeId
                }).OrderBy(e => e.EmployeeId)
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }
              
            return sb.ToString().TrimEnd();
        }

        //Problem 04
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                }).OrderBy(e=>e.FirstName);

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 05
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DeparmentName = e.Department.Name,
                    e.Salary
                })
                .OrderBy(e=>e.Salary)
                .ThenByDescending(e=>e.FirstName)
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DeparmentName} - ${e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 06
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId= 4
            };


            Employee? employee = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");
            employee!.Address = newAddress;

            // context.SaveChanges();

            var employeesAddress = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address!.AddressText);

            foreach (var e in employeesAddress)
            {
                sb.AppendLine($"{e}");
            }

            return sb.ToString().TrimEnd();

        }

        //Problem 07


        //Problem 08
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addresses = context.Addresses
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town!.Name,
                    EmployeeCount = a.Employees.Count()
                })
                .OrderByDescending(a => a.EmployeeCount)
                .ThenBy(a => a.TownName)
                .ThenBy(a => a.AddressText)
                .Take(10);

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();

        }

        //Problem 09
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee147 = context.Employees
                .Find(147);

            sb.AppendLine($"{employee147!.FirstName} {employee147.LastName} - {employee147.JobTitle}");

            var eProjects = context.EmployeesProjects
                .Where(ep => ep.EmployeeId == 147)
                .OrderBy(ep=>ep.Project.Name)
                .ToArray();

            var projects = context.Projects.ToArray();
                

            foreach ( var p in eProjects) 
            {
                if (projects.Where(pr => pr.ProjectId == p.ProjectId)!=null)
                {
                    sb.AppendLine(projects.First(pr => pr.ProjectId == p.ProjectId).Name);
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 10
       public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .Select(d => new
                {
                    EmployeeCont = d.Employees.Count,
                    DepartmentName = d.Name,
                    ManegerFirstName = d.Manager.FirstName,
                    ManegerLastName = d.Manager.LastName,
                    d.Employees,
                })
                .OrderBy(d => d.EmployeeCont)
                .ThenBy(d => d.DepartmentName);

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.DepartmentName} - {d.ManegerFirstName} {d.ManegerLastName}");

                foreach (var e in d.Employees.OrderBy(em=>em.FirstName).ThenBy(em=>em.LastName))
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 11
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var last10Projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt",
                  CultureInfo.InvariantCulture)
                })
                .Take(10);

            foreach (var p in last10Projects.OrderBy(p => p.Name)) 
            {
                sb.AppendLine(p.Name);
                sb.AppendLine(p.Description);
                sb.AppendLine(p.StartDate.ToString());
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 12
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var departmentsToIncrese = new string[]
            {
                "Engineering", "Tool Design", "Marketing","Information Services"
            };
            var employees = context.Employees
                .Where(e => departmentsToIncrese.Contains(e.Department.Name)).OrderBy(e=>e.FirstName).ThenBy(e=>e.LastName);
                
            foreach (var employee in employees) 
            {
                employee.Salary += employee.Salary * (decimal)0.12;
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 13
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var filteredEmployees = context.Employees
                                         .Where(e => e.FirstName.ToLower().StartsWith("sa"))
                                         .OrderBy(e => e.FirstName)
                                         .ThenBy(e => e.LastName)
                                         .Select(e => new
                                         {
                                             e.FirstName,
                                             e.LastName,
                                             e.JobTitle,
                                             e.Salary
                                         });

            // Output the results
            foreach (var e in filteredEmployees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
            }
            
            return sb.ToString().TrimEnd();
        }

        //Problem 14
        public static string DeleteProjectById(SoftUniContext context) 
        {
            var project = context.Projects.Find(2);

            var employeesWithProject = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2);

            foreach (var ep in employeesWithProject)
            {
                context.EmployeesProjects.Remove(ep);
            }
            context.Projects.Remove(project);

            context.SaveChanges();

            var projects = context.Projects
                .Select(p => p.Name)
                .Take(10);

            return string.Join(Environment.NewLine, projects);
        }

        //Problem 15
        public static string RemoveTown(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Address!.Town!.Name == "Seattle");

            var addresses = context.Addresses
                .Where(a => a.Town!.Name == "Seattle");

            int deletedAddressesCount = addresses.Count();

            foreach (var e in employees)
            {
                e.Address!.TownId = null;
            }

            foreach(var a in addresses) 
            {
                context.Addresses.Remove(a);
            }

            context.SaveChanges();

            return $"{deletedAddressesCount} addresses in Seattle were deleted";
        }

    }
}