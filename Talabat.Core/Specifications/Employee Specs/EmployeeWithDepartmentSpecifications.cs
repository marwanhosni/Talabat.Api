using Talabat.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.Employee_Specs;
public class EmployeeWithDepartmentSpecifications : BaseSpecification<Employee>
{
    public EmployeeWithDepartmentSpecifications() :base()
    {
        Include.Add(E => E.depatrment);
    }
    public EmployeeWithDepartmentSpecifications(int id) : base(E => E.Id == id)
    {   
        Include.Add(E => E.depatrment);
        
    }
}
