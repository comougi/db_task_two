using System;
using System.Linq;
using System.Text;
using BdTaskTwo.Data.Models;

namespace BdTaskTwo
{
    internal class Program
    {
        private static readonly EmployeesContext _context = new EmployeesContext();

        private static void Main(string[] args)
        {
            TaskEight();
        }

        private static string GetEmployeesInformation()
        {
            // var employees = _context.Employees.OrderBy(e => e.EmployeeId).Select(e => new
            // {
            //     e.FirstName,
            //     e.LastName,
            //     e.MiddleName,
            //     e.JobTitle
            // }).ToList();

            var employees = (from employee in _context.Employees
                orderby employee.EmployeeId
                select new
                {
                    employee.FirstName,
                    employee.LastName,
                    employee.MiddleName,
                    employee.JobTitle
                }).ToList();

            var sb = new StringBuilder();
            foreach (var e in employees) sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle}");

            return sb.ToString().TrimEnd();
        }

        private static void TaskOne()
        {
            // var employees = _context.Employees.Where(e => e.Salary > 48000).OrderBy(e => e.Salary).Select(e => new
            // {
            //     e.FirstName,
            //     e.LastName,
            //     e.MiddleName,
            //     e.JobTitle,
            //     e.Salary
            // }).ToList();

            var employees = (from employee in _context.Employees
                where employee.Salary > 48000
                orderby employee.Salary
                select new
                {
                    employee.FirstName,
                    employee.LastName,
                    employee.MiddleName,
                    employee.JobTitle,
                    employee.Salary
                }).ToList();
            var sb = new StringBuilder();
            foreach (var e in employees)
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary}");

            Console.WriteLine(sb.ToString().TrimEnd());
        }

        private static void TaskTwo()
        {
            // var employees = _context.Employees.Where(e => e.LastName == "Brown").ToList();

            var employees = (from employee in _context.Employees where employee.LastName == "Brown" select employee)
                .ToList();
            var addresses = new Addresses();
            addresses.AddressText = "New address";
            addresses.TownId = 1;

            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                e.Address = addresses;
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Address.AddressText}");
            }

            _context.SaveChanges();
            Console.WriteLine(sb.ToString().TrimEnd());
        }

        private static void TaskThree()
        {
            var firstDate = new DateTime(2002, 1, 1);
            var lastDate = new DateTime(2005, 12, 31);

            // var res = _context.Employees.Join(_context.EmployeesProjects, e => e.EmployeeId, emp => emp.EmployeeId,
            //     (e, emp) => new
            //     {
            //         emp.EmployeeId,
            //         emp.ProjectId,
            //         e.FirstName,
            //         e.LastName,
            //         e.Manager
            //     }).Join(_context.Projects, e => e.ProjectId, p => p.ProjectId,
            //     (e, p) => new
            //     {
            //         ProjectName = p.Name,
            //         ProjectStartDate = p.StartDate,
            //         ProjectEndDate = p.EndDate,
            //         e.FirstName,
            //         e.LastName,
            //         e.Manager
            //     }).Where(p => p.ProjectStartDate >= firstDate && p.ProjectEndDate >= lastDate).Take(5).ToList();

            var res = (from employee in _context.Employees
                join ep in _context.EmployeesProjects on employee.EmployeeId equals ep.EmployeeId
                join project in _context.Projects on ep.ProjectId equals project.ProjectId
                where project.StartDate >= firstDate && project.EndDate <= lastDate
                select new
                {
                    ep.EmployeeId,
                    ep.ProjectId,
                    employee.FirstName,
                    employee.LastName,
                    employee.Manager,
                    ProjectName = project.Name,
                    ProjectStartDate = project.StartDate,
                    ProjectEndDate = project.EndDate
                }).Take(5).ToList();

            var sb = new StringBuilder();

            foreach (var r in res)
                sb.AppendLine(
                    r.ProjectEndDate == null
                        ? $" {r.FirstName} {r.LastName} {r.Manager.FirstName}\n {r.Manager.LastName} {r.ProjectName} {r.ProjectStartDate} НЕ ЗАВЕРШЁН"
                        : $"{r.FirstName} {r.LastName} {r.Manager.FirstName}\n {r.Manager.LastName} {r.ProjectName} {r.ProjectStartDate} {r.ProjectEndDate}");

            Console.WriteLine(sb.ToString().TrimEnd());
        }

        private static void TaskFour()
        {
            var employeeId = int.Parse(Console.ReadLine());

            // var employees = _context.Employees.Where(e => e.EmployeeId == employeeId).Select(e => new
            // {
            //     e.FirstName,
            //     e.LastName,
            //     e.MiddleName,
            //     e.JobTitle
            // }).ToList();

            var employees = (from employee in _context.Employees
                where employee.EmployeeId == employeeId
                select new
                {
                    employee.FirstName,
                    employee.LastName,
                    employee.MiddleName,
                    employee.JobTitle,
                    employee.Salary
                }).ToList();

            var sb = new StringBuilder();
            foreach (var e in employees) sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle}");

            Console.WriteLine(sb.ToString().TrimEnd());
            ShowProjectById(employeeId);
        }

        private static void ShowProjectById(int employeeId)
        {
            // var projectIds = _context.EmployeesProjects.Where(e => e.EmployeeId == employeeId).Select(e => new
            // {
            //     e.ProjectId
            // }).ToList();

            var projectIds = (from projectId in _context.EmployeesProjects
                where projectId.EmployeeId == employeeId
                select new
                {
                    projectId.ProjectId
                }).ToList();


            var sb = new StringBuilder();
            for (var i = 0; i < projectIds.Count; i++)
            {
                // var projects = _context.Projects.Where(e => e.ProjectId == projectIds.ElementAt(i).ProjectId).Select(
                //     e => new
                //     {
                //         e.Name
                //     }).ToList();

                var projects = (from project in _context.Projects
                    where project.ProjectId == projectIds.ElementAt(i).ProjectId
                    select new
                    {
                        project.Name
                    }).ToList();
                foreach (var e in projects) sb.AppendLine($"{e.Name}");
            }

            Console.WriteLine(sb);
        }

        private static void TaskFive()
        {
            // var depatments = _context.Departments.Where(d => d.Employees.Count() < 5).Select(d => new
            // {
            //     d.Name,
            //     d.Employees
            // }).ToList();

            var departments = (from department in _context.Departments
                where department.Employees.Count() < 5
                select new
                {
                    department.Name,
                    department.Employees
                }).ToList();

            var sb = new StringBuilder();
            foreach (var d in departments) sb.AppendLine($"{d.Name}");

            Console.WriteLine(sb.ToString().TrimEnd());
        }

        private static void TaskSix()
        {
            var departmentTitle = Console.ReadLine();
            var increasePercents = Convert.ToDecimal(Console.ReadLine());

            // var department = _context.Departments.Where(d => d.Name == departmentTitle).Select(d => new
            // {
            //     d.Name,
            //     d.Employees
            // });

            var department = from departments in _context.Departments
                where departments.Name == departmentTitle
                select new
                {
                    departments.Name,
                    departments.Employees
                };

            // var employees = _context.Employees.Where(e => e.Department.Name == departmentTitle).ToList();

            var employees = (from employee in _context.Employees
                where employee.Department.Name == departmentTitle
                select employee).ToList();

            var salaryPercentsAfterIncrease = increasePercents / 100;

            var sb = new StringBuilder();
            foreach (var e in employees)
            {
                var salaryAfterIncreasing = e.Salary;
                e.Salary = salaryAfterIncreasing + salaryAfterIncreasing * salaryPercentsAfterIncrease;
                sb.AppendLine($"{e.FirstName} {e.LastName} New salary: {e.Salary}");
            }

            _context.SaveChanges();
            Console.WriteLine(sb.ToString().TrimEnd());
        }

        private static void TaskEight()
        {
            var townName = Console.ReadLine();

            // var town = _context.Towns.FirstOrDefault(t => t.Name == townName);
            var town = (from towns in _context.Towns where towns.Name == townName select towns).FirstOrDefault();
            // var addresses = _context.Addresses.Where(a => a.Town.Name == townName).ToList();
            var addresses = (from address in _context.Addresses where address.Town.Name == townName select address)
                .ToList();

            foreach (var a in addresses) a.TownId = null;

            _context.Remove(town);
            _context.SaveChanges();
            Console.WriteLine("Town was removed");
        }
    }
}