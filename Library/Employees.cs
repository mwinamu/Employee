using  System;
using System.Linq;
using System.Collections.Generic;

namespace Library
{
    public class Employees
    {
        private DSG<Employee> graph;
        private Dictionary<string, Employee> employees;
        
        
        public Employees(string[] lines)
        {
            graph = new DSG<Employee>();
            employees = new Dictionary<string, Employee>();
            var lns = lines.Select(a => a.Split('\t'));
            var csv = from line in lns
                select (from piece in line
                    select piece);
            int ceos = 0;
            foreach (var n in csv)
            {
                
                var p = n.GetEnumerator();
                while (p.MoveNext())
                {
                    try
                    {
                    var data = p.Current.Split(',');
                    if (string.IsNullOrEmpty(data[0]))
                    {
                        Console.WriteLine("Employee id is empty, this will be skipped");
                        continue;
                    }

                    if (string.IsNullOrEmpty(data[1]) && ceos<1)
                    {
                        ceos ++;
                    }
                    else if (string.IsNullOrEmpty(data[1]) && ceos==1)
                    {
                        Console.WriteLine("There can only be 1 ceo in the organization, skipped");
                        continue;
                    }
                    
                    
                    int sal = 0;
                        // The salaries in the CSV are valid integer numbers
                        if (Int32.TryParse(data[2], out sal))
                    {
                        var employee = new Employee(data[0], data[1], sal);
                        try
                        {
                            employees.Add(employee.Id, employee);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error whilst adding employee to dictionary",e);
                        }
                        
                        if (!graph.HasVertex(employee))
                        {
                            graph.AddVertex(employee);
                        }
                       
                    }
                    else
                    {
                        Console.WriteLine($@"This salary  {data[2]} not a valid integer, skipped");
                    }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                p.Dispose();

            }

            foreach (KeyValuePair<string,Employee> kvp in employees)
            {
                if (!string.IsNullOrEmpty(kvp.Value.Manager))
                {
                    // There is no circular reference, i.e. a first employee reporting to a second employee that is also under the first employee.
                    bool doubleLinked = false;
                    foreach (Employee employee in graph.DepthFirstWalk(kvp.Value).ToArray())
                    {
                        if (employee.Equals(kvp.Value.Manager))
                        {
                            doubleLinked = true;
                            break;
                        }
                    }
                    // ensure that each employee has only one manager
                    if (graph.IncomingEdges(kvp.Value).ToArray().Length < 1 && !doubleLinked )
                    {
                        graph.AddEdge( employees[kvp.Value.Manager],kvp.Value);
                    }
                    else
                    {
                        Console.WriteLine(graph.IncomingEdges(kvp.Value).ToArray().Length>=1 ?
                            String.Format("One employee {0} does not report to more than one manager.", kvp.Value.Id) :
                            "There is no circular reference,");
                    }
                }
               
            }
           
        }

        public long SalaryBudget(string manager)
        {
            var salaryBudget = 0;
            try
            {
                var employeesInPath = graph.DepthFirstWalk(employees[manager]).GetEnumerator();
                while (employeesInPath.MoveNext())
                {
                    salaryBudget += employeesInPath.Current.Salary;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occured when getting salary budget ",e);
            }

            return salaryBudget;
        }
    }

    public class Employee : IComparable<Employee>
    {
        public string Id { get; set; }
        public int Salary { get; set; }
        
        public string Manager { get; set; }

        public Employee(string id,  string manager, int salary)
        {
            Id = id;
            Salary = salary;
            Manager = manager;
        }
        
        public int CompareTo(Employee other)
        {
            if(other == null) return -1;
            return string.Compare(this.Id,other.Id,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}